namespace Hawk.Domain.Entities.Payment
{
    public sealed class Price
    {
        public Price(double value, Currency currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public double Value { get; }

        public Currency Currency { get; }
    }
}