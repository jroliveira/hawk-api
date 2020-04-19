namespace Hawk.WebApi.Features.Currency
{
    using Hawk.Domain.Currency;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;

    public sealed class CurrencyModel
    {
        private CurrencyModel(Currency entity)
        {
            this.Code = entity.Id;
            this.Symbol = entity.Symbol.GetOrElse(Empty);
            this.Transactions = entity.Transactions;
        }

        public string Code { get; }

        public string Symbol { get; }

        public uint Transactions { get; }

        internal static CurrencyModel NewCurrencyModel(Currency entity) => new CurrencyModel(entity);
    }
}
