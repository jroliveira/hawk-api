namespace Hawk.Domain.Entities.Payment
{
    using Hawk.Infrastructure;

    public sealed class Price
    {
        public Price(double value, Currency currency)
        {
            Guard.NotNull(currency, nameof(currency), "Currency cannot be null.");

            this.Value = value;
            this.Currency = currency;
        }

        public double Value { get; }

        public Currency Currency { get; }
    }
}