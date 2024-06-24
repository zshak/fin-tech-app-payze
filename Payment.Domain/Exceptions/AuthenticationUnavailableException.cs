using Payment.App.Exceptions.Base;

namespace Domain.Exceptions;

public class AuthenticationUnavailableException : BaseException
{
    public AuthenticationUnavailableException() : base(503, "Validation Service Unavailable")
    {
    }
}