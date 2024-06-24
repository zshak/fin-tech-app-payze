using Payment.Domain.Enum;

namespace Payment.Domain.Entity;

public class OrderPayment
{
    public int OrderId { get; set; }
    public int CompanyId { get; set; }
    public float AmountToPay { get; set; }
    public float AmountPayed { get; set; }
    public Currency Currency { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}