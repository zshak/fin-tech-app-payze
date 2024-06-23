using Application.Command;
using Application.CQRSbase;
using Domain.Config;
using Domain.Contracts.HelperService;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Command;

public class ComputeOrdersCommandHandler : IRequestHandler<ComputeOrdersCommand,
BaseResponse<ComputeOrdersCommandResponse>>
{
    private readonly IRabbitMqPublisherService _rabbitMqPublisherService;
    private readonly RabbitMqConfig _rabbitMqConfig;
    
    public ComputeOrdersCommandHandler(IOptions<RabbitMqConfig> rabbitMqConfig, IRabbitMqPublisherService rabbitMqPublisherService)
    {
        _rabbitMqPublisherService = rabbitMqPublisherService;
        _rabbitMqConfig = rabbitMqConfig.Value;
    }
    
    public Task<BaseResponse<ComputeOrdersCommandResponse>> Handle(ComputeOrdersCommand request, CancellationToken cancellationToken)
    {
        _rabbitMqPublisherService.Publish(request, "order-exchange", "order-status-queue");
        return Task.FromResult(new BaseResponse<ComputeOrdersCommandResponse>()
        {
            Success = true,
            Message = "Request Accepted, Estimated Wait Time: 2 min",
            Response = new ComputeOrdersCommandResponse()
        });
    }
}