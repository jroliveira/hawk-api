namespace Hawk.Domain.Currency
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    public interface IGetCurrencies
    {
        Task<Try<Page<Try<(Currency Currency, uint Count)>>>> GetResult(Email email, Filter filter);
    }
}
