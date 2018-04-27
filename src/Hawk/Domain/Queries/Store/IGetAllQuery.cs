namespace Hawk.Domain.Queries.Store
{
    using System.Threading.Tasks;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Http.Query.Filter;

    public interface IGetAllQuery
    {
        Task<Try<Paged<Try<(Store Store, uint Count)>>>> GetResult(string email, Filter filter);
    }
}
