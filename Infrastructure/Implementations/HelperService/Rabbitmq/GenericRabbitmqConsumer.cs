using Application.Implementations.HelperService;
using Domain.Config;
using Domain.Contracts.HelperService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Implementations.HelperService.Rabbitmq;

public abstract class GenericRabbitmqConsumer : IRabbitMqConsumerService
{
    protected readonly IModel _channel;
    private readonly string _exchange;
    private readonly RabbitMqConfig _config;

    public GenericRabbitmqConsumer(
        IOptions<RabbitMqConfig> rabbitMqConfig)
    {
        _config = rabbitMqConfig.Value;
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = _config.Host,
            VirtualHost = _config.VirtualHost,
            Port = _config.Port,
            UserName = _config.Username,
            Password = _config.Password,
            AutomaticRecoveryEnabled = true,
            DispatchConsumersAsync = true,
            ConsumerDispatchConcurrency = 10,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(1.0)
        };

        IConnection connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();

        InitializeExchanges();

        _channel.BasicQos(0, 1, false);
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
                    exchangeType == "fanout" ? "" : queue);
            }
        }
    }

    public abstract Task StartAsync(CancellationToken cancellationToken);

    public abstract Task StopAsync(CancellationToken cancellationToken);
}