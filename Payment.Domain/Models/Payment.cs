namespace Payment.Domain.Models;

public class Payment
{
    public int OrderId { get; set; }
    public string CardNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
}