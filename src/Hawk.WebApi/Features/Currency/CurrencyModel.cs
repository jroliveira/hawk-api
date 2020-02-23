namespace Hawk.WebApi.Features.Currency
{
    using Hawk.Domain.Currency;

    public sealed class CurrencyModel
    {
        private CurrencyModel(Currency entity)
        {
            this.Name = entity.Id;
            this.Transactions = entity.Transactions;
        }

        public string Name { get; }

        public uint Transactions { get; }

        internal static CurrencyModel NewCurrencyModel(Currency entity) => new CurrencyModel(entity);
    }
}
