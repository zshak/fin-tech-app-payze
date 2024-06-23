namespace Application.CQRSbase;

public class BaseResponse<T>
{
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
    public required T Response { get; set; }
}