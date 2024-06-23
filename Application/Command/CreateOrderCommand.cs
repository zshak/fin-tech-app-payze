using Application.CQRSbase;
using Domain.Enum;
using MediatR;

namespace Application.Command;

public class CreateOrderCommand : IRequest<BaseResponse<CreateOrderCommandResponse>>
{
    public int CompanyId { get; set; }
    public float Amount { get; set; }
    public Currency Currency { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public class CreateOrderCommandResponse
{
    public int OrderId { get; set; }
}