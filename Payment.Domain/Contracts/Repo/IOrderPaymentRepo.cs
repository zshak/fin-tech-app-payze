using Payment.Domain.Contracts.Repo.Generic;
using Payment.Domain.Entity;

namespace Payment.Domain.Contracts.Repo;

public interface IOrderPaymentRepo : IRepo<OrderPayment>
{
    
}