namespace Hawk.Domain.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    public interface IGetTransactions
    {
        Task<Try<Paged<Try<Transaction>>>> GetResult(Email email, Filter filter);
    }
}
