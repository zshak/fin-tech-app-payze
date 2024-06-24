using System.Text;
using System.Text.Json;
using Application.Command;
using Domain.Config;
using Domain.Contracts.Repo;
using Domain.Enum;
using Domain.Models.Rabbit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Implementations.HelperService.Rabbitmq;

public class PaymentStatusRabbitmqConsumer : GenericRabbitmqConsumer, IDisposable
{
    private readonly string _queueName;
    private readonly ILogger<PaymentStatusRabbitmqConsumer> _logger;
    private IOrderRepo _orderRepo;
    private IOrderComputationStatusRepo _orderComputationStatusRepo;
    private readonly IServiceScope _scopeProvider;

    public PaymentStatusRabbitmqConsumer(
        IOptions<RabbitMqConfig> rabbitMqConfig,
        ILogger<PaymentStatusRabbitmqConsumer> logger,
        IServiceScopeFactory serviceScopeFactory) : base(rabbitMqConfig)
    {
        _scopeProvider = serviceScopeFactory.CreateScope();
        _orderRepo = _scopeProvider.ServiceProvider.GetRequiredService<IOrderRepo>();
        _orderComputationStatusRepo = _scopeProvider.ServiceProvider.GetRequiredService<IOrderComputationStatusRepo>();
        
        _logger = logger;
        _queueName = rabbitMqConfig.Value.PaymentStatusQueue;
    }

    private async Task OnConsumerReceived(object _, BasicDeliverEventArgs ea)
    {
        try
        {
            
            string message = Encoding.UTF8.GetString(ea.Body.ToArray());
            RabbitPublishedPaymentStatus status = JsonSerializer.Deserialize<RabbitPublishedPaymentStatus>(message)!;

            var order = await _orderRepo.GetByIdAsync(status.OrderId);

            order.ComputationStatus = status.Accepted ? OrderStatus.Completed : OrderStatus.Rejected;

            await _orderRepo.UpdateAsync(order);
            
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
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }
}