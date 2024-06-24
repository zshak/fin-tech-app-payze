using System.Text;
using System.Text.Json;
using Application.Implementations.HelperService;
using Domain.Config;
using Domain.Contracts.HelperService;
using Domain.Contracts.Repo;
using Domain.Entity;
using Domain.Models.Order;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.HostedServices;

public class OrderPublisherJob : BackgroundService
{
    private readonly IRabbitMqPublisherService _rabbitMqPublisherService;
    private readonly IModel _channel;
    private readonly ILogger<OrderPublisherJob> _logger;
    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public OrderPublisherJob(
        IOptions<RabbitMqConfig> rabbitMqConfig, 
        ILogger<OrderPublisherJob> logger,
        IServiceScopeFactory serviceScopeFactory, 
        IRabbitMqPublisherService rabbitMqPublisherService)
    {
        
        
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _rabbitMqPublisherService = rabbitMqPublisherService;
        _rabbitMqConfig = rabbitMqConfig.Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepo>();
            try
            {
                List<Order> unpublishedOrders = await orderRepo.GetUnpublishedOrdersForUpdateAsync();
                unpublishedOrders.ForEach(order => _rabbitMqPublisherService.Publish(
                    new PublishedOrder()
                    {
                        Amount = order.Amount,
                        CompanyId = order.CompanyId,
                        CreatedAtUtc = order.CreatedAtUtc,
                        Currency = order.Currency,
                        OrderId = order.OrderId
                    },"order-exchange", "payment-queue"));

                await orderRepo.SetOrdersToPublishedAsync(unpublishedOrders.Select(o => o.OrderId).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError($"Order Publishing Failed. Message: {e.Message}, StackTrace: {e.StackTrace}");
            }
            await Task.Delay(TimeSpan.FromSeconds(15));
        }

    }

    private void Publish(Order order)
    {
        byte[] serialized = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new PublishedOrder()
        {
            Amount = order.Amount,
            CompanyId = order.CompanyId,
            CreatedAtUtc = order.CreatedAtUtc,
            Currency = order.Currency,
            OrderId = order.OrderId
        }));

        lock (_channel)
        {
            _channel.BasicPublish(_rabbitMqConfig.OrderExchange, "",body: serialized);
        }
    }
}