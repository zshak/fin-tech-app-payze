namespace Domain.Contracts.Strategy;

public interface IApiKeyStrategy
{
    public bool IsValidApiKey(Guid correctKey, Guid providedKey);
    public bool IsValidApiSecret(string correctKey, string providedKey);
    public Guid GenerateApiKey();
    public string GenerateApiSecret();
}