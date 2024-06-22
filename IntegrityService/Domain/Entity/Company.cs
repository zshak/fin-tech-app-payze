namespace Domain.Models.Entity;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Guid ApiKey { get; set; }
    public string ApiSecret { get; set; }
}