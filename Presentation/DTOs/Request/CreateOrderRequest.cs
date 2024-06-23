using Domain.Enum;

namespace Presentation.DTOs.Request;

public class CreateOrderRequest
{
    public float Amount { get; set; }
    public Currency Currency { get; set; }
}