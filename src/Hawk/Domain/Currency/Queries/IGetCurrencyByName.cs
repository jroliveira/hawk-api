namespace Hawk.Domain.Currency.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetCurrencyByName : IQuery<GetByIdParam<string>, Currency>
    {
    }
}
