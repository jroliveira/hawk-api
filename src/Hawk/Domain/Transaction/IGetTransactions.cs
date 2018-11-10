namespace Hawk.Domain.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    public interface IGetTransactions
    {
        Task<Try<Paged<Transaction>>> GetResult(string email, Filter filter);
    }
}
