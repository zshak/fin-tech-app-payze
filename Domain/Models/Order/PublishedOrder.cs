using Domain.Enum;

namespace Domain.Models.Order;

public class PublishedOrder
{
    public int OrderId { get; set; }
    public int CompanyId { get; set; }
    public float Amount { get; set; }
    public Currency Currency { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    
}