using Domain.Contracts.Generic;
using Domain.Entity;

namespace Domain.Contracts.Repo;

public interface IOrderRepo : IRepo<Order>
{
    public Task<int> AddOrderAsync(Order order);
    public Task<List<Order>> GetUnpublishedOrdersForUpdateAsync();
    public Task SetOrdersToPublishedAsync(List<int> orderIds);
    public Task<float> GetTotalOrderAmount(int companyId);
}