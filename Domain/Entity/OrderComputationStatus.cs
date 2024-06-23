using Domain.Enum;

namespace Domain.Entity;

public class OrderComputationStatus
{
    public Guid OrderComputationRequestGuid { get; set; }
    public int CompanyId { get; set; }
    public OrderComputationProgress Status { get; set; }
    public float ComputationResult { get; set; }
}