using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;


public class OrderContext : DbContext
{
    private readonly IConfiguration _configuration;

    public OrderContext(IConfiguration config)
    {
        _configuration = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderComputationStatus> OrderComutationStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderComputationStatus>(entity =>
        {
            entity.HasKey(x => x.OrderComputationRequestGuid);
            
            entity.Property(x => x.OrderComputationRequestGuid)
                .IsRequired();
            
            entity.Property(x => x.CompanyId)
                .IsRequired();
            
            entity.Property(x => x.Status)
                .IsRequired();
        });
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(x => x.OrderId);
            entity.Property(x => x.OrderId)
                .ValueGeneratedOnAdd();

            entity
                .Property(e => e.CompanyId)
                .IsRequired();

            entity
                .Property(e => e.Amount)
                .IsRequired();

            entity
                .Property(e => e.Currency)
                .IsRequired();
            
            entity
                .Property(e => e.ComputationStatus)
                .IsRequired();
            
            entity
                .Property(e => e.CreatedAtUtc)
                .IsRequired();
        });
    }
}