using Application.CQRSbase;
using Application.Query;
using Domain.Contracts.Repo;
using Domain.Entity;
using Domain.Enum;
using MediatR;

namespace Application.Handlers.Query;

public class OrderComputationStatusQueryHandler : IRequestHandler<OrderComputationStatusQuery,
BaseResponse<OrderComputationStatusQueryResponse>>
{
    public IOrderComputationStatusRepo _orderComputationStatusRepo;
    public OrderComputationStatusQueryHandler(IOrderComputationStatusRepo orderComputationStatusRepo)
    {
        _orderComputationStatusRepo = orderComputationStatusRepo;
    }
    
    public async Task<BaseResponse<OrderComputationStatusQueryResponse>> Handle(OrderComputationStatusQuery request, CancellationToken cancellationToken)
    {
        var orderComputationStatus = await _orderComputationStatusRepo.GetByGuidAsync(request.OrderComputationId);
        if (orderComputationStatus == null || orderComputationStatus.Status == OrderComputationProgress.InProgress)
        {
            return new BaseResponse<OrderComputationStatusQueryResponse>()
            {
                Success = true,
                Message = "Order Computation In Progress",
                Response = new OrderComputationStatusQueryResponse()
                {
                    Status = OrderComputationProgress.InProgress
                }
            };
        }

        return new BaseResponse<OrderComputationStatusQueryResponse>()
        {
            Success = true,
            Message = "Order Computation Finished",
            Response = new OrderComputationStatusQueryResponse()
            {
                Status = OrderComputationProgress.Finished,
                Value = orderComputationStatus.ComputationResult
            }
        };
    }
}