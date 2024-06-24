using Payment.App.Exceptions.Base;

namespace Payment.Domain.Exceptions;

public class InvalidCardNumberException :BaseException
{
    public InvalidCardNumberException() : base(400, "Invalid Card Number")
    {
    }
    
    public InvalidCardNumberException(string validationFailures) : base(400, validationFailures)
    {
    }
}