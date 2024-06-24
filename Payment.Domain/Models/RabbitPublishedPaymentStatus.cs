namespace Payment.Domain.Models;

public class RabbitPublishedPaymentStatus
{
    public int OrderId { get; set; }
    public int CompanyId { get; set; }
    public bool Accepted { get; set; }
}