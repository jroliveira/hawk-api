namespace Hawk.Domain.Queries.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure;

    using Http.Query.Filter;

    public interface IGetAllQuery
    {
        Task<Paged<(Method Method, int Count)>> GetResult(string email, Filter filter);
    }
}