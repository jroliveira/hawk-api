namespace Hawk.Domain.Queries.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure;

    using Http.Query.Filter;

    public interface IGetAllByStoreQuery
    {
        Task<Paged<Method>> GetResult(string email, string store, Filter filter);
    }
}
