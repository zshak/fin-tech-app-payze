namespace Application.Models;

public class IdentityServiceAuthenticationModel
{
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}