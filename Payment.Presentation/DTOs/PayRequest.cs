namespace Payment.Presentation.DTOs;

public class PayRequest
{
    public int OrderId { get; set; }
    public string CardNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
}