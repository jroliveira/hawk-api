namespace Hawk.WebApi.Features.Currency
{
    using System.Linq;

    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using static Hawk.Domain.Currency.Currency;
    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    public sealed class CurrencyModel
    {
        public CurrencyModel(Currency entity)
            : this(entity, default)
        {
        }

        public CurrencyModel(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }

        public static implicit operator Option<Currency>(CurrencyModel model) => NewCurrency(model.Name);

        internal static Try<Page<Try<CurrencyModel>>> MapCurrency(Page<Try<(Currency Currency, uint Count)>> @this) => new Page<Try<CurrencyModel>>(
            @this
                .Data
                .Select(item => item.Match(
                    HandleException<CurrencyModel>,
                    currency => new CurrencyModel(currency.Currency, currency.Count))),
            @this.Skip,
            @this.Limit);
    }
}
