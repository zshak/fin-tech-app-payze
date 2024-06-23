using Domain.Enum;

namespace Domain.Entity;

public class Order
{
    public int OrderId { get; set; }
    public int CompanyId { get; set; }
    public float Amount { get; set; }
    public Currency Currency { get; set; }
    public OrderStatus ComputationStatus { get; set; } 
    public DateTime CreatedAtUtc { get; set; }
    public bool Notified { get; set; }
}