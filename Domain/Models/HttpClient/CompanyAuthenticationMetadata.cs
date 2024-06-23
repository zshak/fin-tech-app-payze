namespace Domain.Models.HttpClient;

public class CompanyAuthenticationMetadata
{
    public int Id { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}