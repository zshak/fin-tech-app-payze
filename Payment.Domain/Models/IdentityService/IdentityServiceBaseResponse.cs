namespace Application.Models.IdentityService;

public class IdentityServiceBaseResponse<T>
{
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
    public required T Response { get; set; }
}