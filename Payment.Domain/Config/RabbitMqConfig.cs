namespace Payment.Domain.Config;

public class RabbitMqConfig
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string VirtualHost { get; set; }
    
    
    public Dictionary<string, ExchangeConfig> Exchanges { get; set; }
    
    public required string OrderExchange { get; set; }
    
    public required string PaymentQueue { get; set; }
    public required string PaymentStatusQueue { get; set; }
}

public class ExchangeConfig
{
    public string Type { get; set; }
    public List<string> Queues { get; set; }
}