using Domain.Contracts.Generic;
using Domain.Entity;
using Domain.Enum;

namespace Domain.Contracts.Repo;

public interface IOrderComputationStatusRepo : IRepo<OrderComputationStatus>
{
    public ValueTask<OrderComputationStatus?> GetByGuidAsync(Guid orderComputationId);
}