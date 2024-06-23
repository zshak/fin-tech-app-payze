using Application.CQRSbase;
using Domain.Entity;
using Domain.Enum;
using MediatR;

namespace Application.Query;

public class OrderComputationStatusQuery : IRequest<BaseResponse<OrderComputationStatusQueryResponse>>
{
    public Guid OrderComputationId { get; set; }
}

public class OrderComputationStatusQueryResponse
{
    public required OrderComputationProgress Status { get; set; }
    public float Value { get; set; }
}