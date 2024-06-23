namespace Domain.Contracts.HelperService;

public interface IRabbitMqPublisherService
{
    public void Publish<T>(T message, string exchange, string routingKey);
}