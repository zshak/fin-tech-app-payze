using Payment.Domain.Strategy;

namespace Payment.Application.Implementations.Strategy;

public class ServiceASenderStrategy : IPaymentServiceSenderStrategy
{
    public async Task<bool> SendToPaymentServiceAsync(Domain.Models.Payment payment)
    {
        await Task.Delay(10);
        Console.WriteLine("Processed By Service A");
        return new Random().Next(0, int.MaxValue) % 2 == 0;
    }
}