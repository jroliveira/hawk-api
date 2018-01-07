namespace Hawk.Domain.Queries.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure;

    using Http.Query.Filter;

    public interface IGetAllByStoreQuery
    {
        Task<Paged<Tag>> GetResult(string email, string store, Filter filter);
    }
}
