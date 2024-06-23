using Domain.Contracts.Repo;
using Domain.Entity;
using Infrastructure.Implementations.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repo.Generic;

public class OrderRepo : Repo<Order>, IOrderRepo
{
    private readonly OrderContext _orderContext; 
    public OrderRepo(OrderContext context, 
        OrderContext orderContext) : base(context)
    {
        _orderContext = orderContext;
    }

    public async Task<int> AddOrderAsync(Order order)
    {
        _orderContext.Orders.Add(order);
        await _orderContext.SaveChangesAsync();
        return order.OrderId;
    }

    public async Task<List<Order>> GetUnpublishedOrdersForUpdateAsync()
    {
        using var transaction = await _orderContext.Database.BeginTransactionAsync();

        try
        {
            var orders = await _orderContext.Orders
                .FromSqlRaw("SELECT * FROM \"Orders\" WHERE \"Notified\" = FALSE FOR UPDATE")
                .ToListAsync();

            await transaction.CommitAsync();
            return orders;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task SetOrdersToPublishedAsync(List<int> orderIds)
    {
        var orders = await _orderContext.Orders
            .Where(order => orderIds.Contains(order.OrderId))
            .ToListAsync();
        orders.ForEach(x => x.Notified = true);
        await _orderContext.SaveChangesAsync();
    }

    public async Task<float> GetTotalOrderAmount(int companyId)
    {
        return _orderContext.Orders
            .Where(order => order.CompanyId == companyId)
            .Sum(order => order.Amount);
    }
}