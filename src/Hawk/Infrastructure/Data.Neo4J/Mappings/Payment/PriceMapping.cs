namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using Hawk.Domain.Entities.Payment;

    internal sealed class PriceMapping
    {
        private readonly CurrencyMapping currencyMapping;

        public PriceMapping(CurrencyMapping currencyMapping)
        {
            Guard.NotNull(currencyMapping, nameof(currencyMapping), "Currency mapping cannot be null.");

            this.currencyMapping = currencyMapping;
        }

        public Price MapFrom(Record record)
        {
            Guard.NotNull(record, nameof(record), "Price's record cannot be null.");

            var currency = this.currencyMapping.MapFrom(record.GetRecord("currency"));

            return new Price(
                record.Get<double>("value"),
                currency);
        }
    }
}