namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using Hawk.Domain.Entities.Payment;

    internal sealed class CurrencyMapping
    {
        public Currency MapFrom(Record record)
        {
            if (record == null || !record.Any())
            {
                return null;
            }

            return new Currency(record.Get("name"));
        }
    }
}