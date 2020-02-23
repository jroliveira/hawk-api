namespace Hawk.Domain.Currency.Queries
{
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    public interface IGetCurrencies : IQuery<GetAllParam, Page<Try<Currency>>>
    {
    }
}
