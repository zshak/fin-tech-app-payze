using MediatR;
using Payment.Application.CQRSbase;

namespace Payment.Application.Command;

public class PayForOrderCommand : IRequest<BaseResponse<PayForOrderCommandResponse>>
{
    public int CompanyId { get; set; }
    public int OrderId { get; set; }
    public string CardNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
}

public class PayForOrderCommandResponse
{
    public required bool PaymentAccepted { get; set; }
}