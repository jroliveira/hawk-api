namespace Hawk.Domain.Queries.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure;

    using Http.Query.Filter;

    public interface IGetAllQuery
    {
        Task<Paged<(Store Store, int Count)>> GetResult(string email, Filter filter);
    }
}
