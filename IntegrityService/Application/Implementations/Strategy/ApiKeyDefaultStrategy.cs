using System.Security.Cryptography;
using Domain.Contracts.Strategy;

namespace Application.Implementations.Strategy;

public class ApiKeyDefaultStrategy : IApiKeyStrategy
{
    public bool IsValidApiKey(Guid correctKey, Guid providedKey)
    {
        return correctKey == providedKey;
    }

    public bool IsValidApiSecret(string correctKey, string providedKey)
    {
        return correctKey == providedKey;
    }

    public Guid GenerateApiKey()
    {
        return Guid.NewGuid();
    }

    public string GenerateApiSecret()
    {
        using var hmac = new HMACSHA256();
        var secretBytes = hmac.Key;
        return Convert.ToBase64String(secretBytes);
    }
}