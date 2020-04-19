namespace Hawk.Domain.Currency.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetCurrencyByCode : IQuery<GetByIdParam<string>, Currency>
    {
    }
}
