using Application.Command;
using Application.CQRSbase;
using Domain.Contracts;
using Domain.Contracts.Repo;
using Domain.Entity;
using Domain.Enum;
using MediatR;

namespace Application.Handlers.Command;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand,
BaseResponse<CreateOrderCommandResponse>>
{
    private readonly IOrderRepo _orderRepo;
    public CreateOrderCommandHandler(IOrderRepo orderRepo)
    {
        _orderRepo = orderRepo;
    }
    public async Task<BaseResponse<CreateOrderCommandResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        int id = await _orderRepo.AddOrderAsync(new Order()
        {
            CompanyId = request.CompanyId,
            CreatedAtUtc = request.CreatedAtUtc,
            Amount = request.Amount,
            Currency = request.Currency,
            ComputationStatus = OrderStatus.Initialized,
            Notified = false
        });
        
        return new BaseResponse<CreateOrderCommandResponse>()
        {
            Success = true,
            Message = "Order Created Successfully",
            Response = new CreateOrderCommandResponse()
            {
                OrderId = id
            }
        };
    }
}