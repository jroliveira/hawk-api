namespace Hawk.Domain.Queries.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    public interface IGetAllQuery
    {
        Task<Try<Paged<Transaction>>> GetResult(string email, Filter filter);
    }
}
