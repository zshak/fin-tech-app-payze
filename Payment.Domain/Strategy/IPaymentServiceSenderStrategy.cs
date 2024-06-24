namespace Payment.Domain.Strategy;

public interface IPaymentServiceSenderStrategy
{
    public Task<bool> SendToPaymentServiceAsync(Models.Payment payment);
}