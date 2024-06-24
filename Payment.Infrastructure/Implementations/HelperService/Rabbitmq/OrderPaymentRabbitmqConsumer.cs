using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.Domain.Config;
using Payment.Domain.Contracts.Repo;
using Payment.Domain.Entity;
using Payment.Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Payment.Infrastructure.Implementations.HelperService.Rabbitmq;

public class OrderPaymentRabbitmqConsumer : GenericRabbitmqConsumer, IDisposable
{
    private readonly string _queueName;
    private readonly ILogger<OrderPaymentRabbitmqConsumer> _logger;
    private IOrderPaymentRepo _orderPaymentRepo;
    private readonly IServiceScope _scopeProvider;

    public OrderPaymentRabbitmqConsumer(
        IOptions<RabbitMqConfig> rabbitMqConfig,
        ILogger<OrderPaymentRabbitmqConsumer> logger,
        IServiceScopeFactory serviceScopeFactory) : base(rabbitMqConfig)
    {
        _scopeProvider = serviceScopeFactory.CreateScope();
        _orderPaymentRepo = _scopeProvider.ServiceProvider.GetRequiredService<IOrderPaymentRepo>();
        
        _logger = logger;
        _queueName = rabbitMqConfig.Value.PaymentQueue;
    }
    

    private async Task OnConsumerReceived(object _, BasicDeliverEventArgs ea)
    {
        try
        {
            string message = Encoding.UTF8.GetString(ea.Body.ToArray());
            PublishedOrder order = JsonSerializer.Deserialize<PublishedOrder>(message)!;

            var orderPayment = await _orderPaymentRepo.GetByIdAsync(order.OrderId);

            if (orderPayment != null)
            {
                //idempotency
                _channel.BasicAck(ea.DeliveryTag, false);
                return; 
            }

            OrderPayment newOrderPayment = new OrderPayment()
            {
                OrderId = order.OrderId,
                AmountToPay = order.Amount,
                CompanyId = order.CompanyId,
                Currency = order.Currency,
                AmountPayed = 0,
                CreatedAtUtc = order.CreatedAtUtc
            };
            await _orderPaymentRepo.AddAsync(newOrderPayment);
            
            _channel.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"{nameof(OrderPaymentRabbitmqConsumer)} Consuming Failed: {e.Message}, {e.StackTrace}");
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
            _logger.LogError($"{nameof(OrderPaymentRabbitmqConsumer)} Failed: {e.Message}, {e.StackTrace}");
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