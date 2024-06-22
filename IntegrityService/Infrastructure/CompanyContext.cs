using Domain.Models.Entity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;
using Microsoft.EntityFrameworkCore;

public class CompanyContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CompanyContext(IConfiguration config)
    {
        _configuration = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }
    
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity
                .Property(e => e.ApiKey)
                .IsRequired();

            entity
                .Property(e => e.ApiSecret)
                .IsRequired();

            entity
                .Property(e => e.Name)
                .IsRequired();
        });
    }
}