using Payment.Domain.Contracts.Repo;
using Payment.Domain.Contracts.Repo.Generic;
using Payment.Domain.Entity;
using Payment.Infrastructure.Implementations.Repo.Generic;

namespace Payment.Infrastructure.Implementations.Repo;

public class OrderPaymentRepo : Repo<OrderPayment>, IOrderPaymentRepo
{
    public OrderPaymentRepo(PaymentContext context) : base(context)
    {
    }
}