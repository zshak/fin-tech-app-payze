using System.Text;
using System.Text.Json;
using Domain.Config;
using Domain.Contracts.HelperService;
using Domain.Models.Order;
using Infrastructure.HostedServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Application.Implementations.HelperService;

public class RabbitMqPublisherService : IRabbitMqPublisherService
{
    private readonly IModel _channel;
    private readonly ILogger<RabbitMqPublisherService> _logger;
    private readonly string _exchange;
    private readonly RabbitMqConfig _config;
    public RabbitMqPublisherService(
        IOptions<RabbitMqConfig> rabbitMqConfig)
    {
        _config = rabbitMqConfig.Value;
        try
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = _config.Host,
                VirtualHost = _config.VirtualHost,
                Port = _config.Port,
                UserName = _config.Username,
                Password = _config.Password
            };

            IConnection connection = connectionFactory.CreateConnection();

            _channel = connection.CreateModel();
            InitializeExchanges();
        }
        catch (Exception e)
        {
            _logger.LogError($"Rabbit Publisher Failed. Message: {e.Message}, StackTrace: {e.StackTrace}");
            throw;
        }
    }   
    
    private void InitializeExchanges()
    {
        foreach (var exchangeEntry in _config.Exchanges)
        {
            string exchangeName = exchangeEntry.Key;
            string exchangeType = exchangeEntry.Value.Type;
            List<string> queues = exchangeEntry.Value.Queues;

            _channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);

            foreach (var queue in queues)
            {
                _channel.QueueDeclare(queue, exclusive: false, durable: true, autoDelete: false);

                _channel.QueueBind(queue: queue, exchange: exchangeName, routingKey: 
                    exchangeType == "fanout"? "" : queue);
            }
        }
    }

    public void Publish<T>(T message, string exchange, string routingKey)
    {
        byte[] serialized = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        lock (_channel)
        {
            var properties = _channel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            _channel.BasicPublish(exchange: exchange, routingKey,body: serialized, basicProperties: properties);
        };
    }
}