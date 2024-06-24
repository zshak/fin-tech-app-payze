using Payment.Domain.Strategy;

namespace Payment.Domain.Factory;

public interface IPaymentServiceSenderStrategyFactory
{
    public IPaymentServiceSenderStrategy GetStrategy(string cardNumber);
}