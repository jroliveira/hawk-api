namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using Hawk.Domain.Entities.Payment;

    internal sealed class PriceMapping
    {
        private readonly CurrencyMapping currencyMapping;

        public PriceMapping(CurrencyMapping currencyMapping)
        {
            this.currencyMapping = currencyMapping;
        }

        public Price MapFrom(Record record)
        {
            var currency = this.currencyMapping.MapFrom(record.GetRecord("currency"));

            return new Price(
                record.Get<double>("value"),
                currency);
        }
    }
}