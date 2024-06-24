using Domain.Contracts.HelperService;
using MediatR;
using Microsoft.Extensions.Options;
using Payment.Application.Command;
using Payment.Application.CQRSbase;
using Payment.Domain.Config;
using Payment.Domain.Contracts.Repo;
using Payment.Domain.Exceptions;
using Payment.Domain.Factory;
using Payment.Domain.Models;

namespace Payment.Application.Handlers;

public class PayForOrderCommandHandler : IRequestHandler<PayForOrderCommand,
BaseResponse<PayForOrderCommandResponse>>
{
    public IPaymentServiceSenderStrategyFactory _strategyFactory;
    public IRabbitMqPublisherService _rabbitMqPublisher;
    public IOrderPaymentRepo _OrderPaymentRepo;
    private readonly RabbitMqConfig _rabbitMqConfig;
    public PayForOrderCommandHandler(
        IPaymentServiceSenderStrategyFactory strategyFactory, 
        IRabbitMqPublisherService rabbitMqPublisher,
        IOrderPaymentRepo orderPaymentRepo, 
        IOptions<RabbitMqConfig> rabbitMqConfig)
    {
        _strategyFactory = strategyFactory;
        _rabbitMqPublisher = rabbitMqPublisher;
        _OrderPaymentRepo = orderPaymentRepo;
        _rabbitMqConfig = rabbitMqConfig.Value;
    }

    public async Task<BaseResponse<PayForOrderCommandResponse>> Handle(PayForOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _OrderPaymentRepo.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new OrderDoesNotExistException();
        }
        
        var isSuccess = await _strategyFactory.GetStrategy(request.CardNumber).SendToPaymentServiceAsync(new Domain.Models.Payment()
        {
            CardNumber = request.CardNumber,
            ExpiryDate = request.ExpiryDate,
            OrderId = request.OrderId
        });
        
        _rabbitMqPublisher.Publish(new RabbitPublishedPaymentStatus()
        {
            CompanyId = request.CompanyId,
            OrderId = request.OrderId,
            Accepted = isSuccess
        }, _rabbitMqConfig.OrderExchange, _rabbitMqConfig.PaymentStatusQueue);
        
        if (isSuccess)
        {
            return new BaseResponse<PayForOrderCommandResponse>()
            {
                Message = "Payment Accepted",
                Success = true,
                Response = new PayForOrderCommandResponse()
                {
                    PaymentAccepted = true
                }
            };
        }

        return new BaseResponse<PayForOrderCommandResponse>()
        {
            Message = "Payment Rejected",
            Success = true,
            Response = new PayForOrderCommandResponse()
            {
                PaymentAccepted = false
            }
        };
    }
}