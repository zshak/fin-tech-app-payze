using System.Text;
using System.Text.Json;
using Application.Command;
using Domain.Config;
using Domain.Contracts.Repo;
using Domain.Entity;
using Domain.Enum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Implementations.HelperService.Rabbitmq;

public class OrderComputationRabbitmqConsumer : GenericRabbitmqConsumer, IDisposable
{
    private readonly string _queueName;
    private readonly ILogger<OrderComputationRabbitmqConsumer> _logger;
    private IOrderRepo _orderRepo;
    private IOrderComputationStatusRepo _orderComputationStatusRepo;
    private readonly IServiceScope _scopeProvider;

    public OrderComputationRabbitmqConsumer(
        IOptions<RabbitMqConfig> rabbitMqConfig,
        ILogger<OrderComputationRabbitmqConsumer> logger,
        IServiceScopeFactory serviceScopeFactory) : base(rabbitMqConfig)
    {
        _scopeProvider = serviceScopeFactory.CreateScope();
        _orderRepo = _scopeProvider.ServiceProvider.GetRequiredService<IOrderRepo>();
        _orderComputationStatusRepo = _scopeProvider.ServiceProvider.GetRequiredService<IOrderComputationStatusRepo>();
        
        _logger = logger;
        _queueName = rabbitMqConfig.Value.OrderStatusQueue;
    }
    
    private async Task OnConsumerReceived(object _, BasicDeliverEventArgs ea)
    {
        try
        {
            string message = Encoding.UTF8.GetString(ea.Body.ToArray());
            ComputeOrdersCommand order = JsonSerializer.Deserialize<ComputeOrdersCommand>(message)!;

            var orderComputationStatus = await _orderComputationStatusRepo.GetByGuidAsync(order.ComputationId);

            if (orderComputationStatus != null)
            {
                _channel.BasicAck(ea.DeliveryTag, false);
                return; 
            }

            OrderComputationStatus newOrderComputationStatus = new OrderComputationStatus()
            {
                OrderComputationRequestGuid = order.ComputationId,
                CompanyId = order.CompanyId,
                ComputationResult = -1,
                Status = OrderComputationProgress.InProgress
            };
            await _orderComputationStatusRepo.AddAsync(newOrderComputationStatus);

            await Task.Delay(TimeSpan.FromSeconds(20));

            var totalAmount = await _orderRepo.GetTotalOrderAmount(order.CompanyId);

            newOrderComputationStatus.ComputationResult = totalAmount;
            newOrderComputationStatus.Status = OrderComputationProgress.Finished;
            
            await _orderComputationStatusRepo.UpdateAsync(newOrderComputationStatus);
            
            _channel.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"{nameof(OrderComputationRabbitmqConsumer)} Consuming Failed: {e.Message}, {e.StackTrace}");
            _channel.BasicNack(ea.DeliveryTag, false, true);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {        
        try
        {
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += OnConsumerReceived;
            _channel.BasicConsume(_queueName, false, consumer);
        }
        catch (Exception e)
        {
            _logger.LogError($"{nameof(OrderComputationRabbitmqConsumer)} Failed: {e.Message}, {e.StackTrace}");
            throw;
        }

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _scopeProvider?.Dispose();
    }
}