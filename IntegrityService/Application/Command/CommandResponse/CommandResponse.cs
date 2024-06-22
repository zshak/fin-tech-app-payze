namespace Application.Command.CommandResponse;

public class CommandResponse<T>
{
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
    public required T Response { get; set; }
}