using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;

namespace Domain.Contracts.HelperService;

public interface IRabbitMqConsumerService : IHostedService
{
}