using Payment.App.Exceptions.Base;

namespace Payment.Domain.Exceptions;

public class OrderDoesNotExistException : BaseException
{
    public OrderDoesNotExistException() : base(400, "Order Does Not Exist")
    {
    }
}