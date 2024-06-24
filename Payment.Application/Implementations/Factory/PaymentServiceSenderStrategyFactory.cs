using Payment.Application.Implementations.Strategy;
using Payment.Domain.Factory;
using Payment.Domain.Strategy;

namespace Payment.Application.Implementations.Factory;

public class PaymentServiceSenderStrategyFactory : IPaymentServiceSenderStrategyFactory
{
    public IPaymentServiceSenderStrategy GetStrategy(string cardNumber)
    {
        
        char lastChar = cardNumber[^1];
        int lastNum =  int.Parse(lastChar.ToString());

        if (lastNum % 2 == 0)
        {
            return new ServiceASenderStrategy();
        }

        return new ServiceBSenderStrategy();
    }
    
}
