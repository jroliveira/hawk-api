namespace Hawk.Domain.Transaction.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetTransactions : IQuery<GetAllParam, Page<Try<Transaction>>>
    {
    }
}
