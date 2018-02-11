namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using Hawk.Domain.Entities.Payment;

    internal sealed class CurrencyMapping
    {
        public Currency MapFrom(Record record)
        {
            Guard.NotNull(record, nameof(record), "Currency's record cannot be null.");

            return new Currency(record.Get("name"));
        }
    }
}