using Domain.Contracts.Repo;
using Domain.Entity;
using Domain.Enum;
using Infrastructure.Implementations.Generic;

namespace Infrastructure.Implementations.Repo;

public class OrderComputationStatusRepo : Repo<OrderComputationStatus>, IOrderComputationStatusRepo
{
    private readonly OrderContext _context;
    public OrderComputationStatusRepo(OrderContext context) : base(context)
    {
        _context = context;
    }

    public ValueTask<OrderComputationStatus?> GetByGuidAsync(Guid orderComputationId)
    {
        return _context.OrderComutationStatuses.FindAsync(orderComputationId);
    }
}