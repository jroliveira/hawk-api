namespace Hawk.Domain.Queries.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    public interface IGetAllByStoreQuery
    {
        Task<Try<Paged<Try<(Tag Tag, uint Count)>>>> GetResult(string email, string store, Filter filter);
    }
}
