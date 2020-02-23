namespace Hawk.Domain.PaymentMethod.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetPaymentMethods : IQuery<GetAllParam, Page<Try<PaymentMethod>>>
    {
    }
}
