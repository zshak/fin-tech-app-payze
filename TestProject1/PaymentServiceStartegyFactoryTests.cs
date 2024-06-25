using Payment.Application.Implementations.Factory;
using Payment.Application.Implementations.Strategy;

namespace TestProject1;

public class Tests
{
    private PaymentServiceSenderStrategyFactory _factory;

    [SetUp]
    public void SetUp()
    {
        _factory = new PaymentServiceSenderStrategyFactory();
    }

    [TestCase("1234567812345678")]
    [TestCase("8765432187654320")] 
    [TestCase("1111111111111112")] 
    public void GetStrategy_ShouldReturnServiceASenderStrategy_ForEvenLastDigit(string cardNumber)
    {
        // Act
        var strategy = _factory.GetStrategy(cardNumber);

        // Assert
        Assert.IsInstanceOf<ServiceASenderStrategy>(strategy);
    }
    
    [TestCase("1234567812345679")]
    [TestCase("8765432187654321")] 
    [TestCase("1111111111111113")]
    public void GetStrategy_ShouldReturnServiceBSenderStrategy_ForOddLastDigit(string cardNumber)
    {
        // Act
        var strategy = _factory.GetStrategy(cardNumber);

        // Assert
        Assert.IsInstanceOf<ServiceBSenderStrategy>(strategy);
    }
}