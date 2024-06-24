using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Payment.Domain.Entity;

namespace Payment.Infrastructure;


public class PaymentContext : DbContext
{
    private readonly IConfiguration _configuration;

    public PaymentContext(IConfiguration config)
    {
        _configuration = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }
    
    public DbSet<OrderPayment> OrderPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderPayment>(entity =>
        {
            entity.HasKey(x => x.OrderId);

            entity
                .Property(e => e.CompanyId)
                .IsRequired();

            entity
                .Property(e => e.AmountToPay)
                .IsRequired();

            entity
                .Property(e => e.AmountPayed)
                .IsRequired();
            
            entity
                .Property(e => e.Currency)
                .IsRequired();
            
            entity
                .Property(e => e.CreatedAtUtc)
                .IsRequired();
        });
    }
}