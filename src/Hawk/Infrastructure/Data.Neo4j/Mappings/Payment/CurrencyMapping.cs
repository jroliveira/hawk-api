namespace Hawk.Infrastructure.Data.Neo4j.Mappings.Payment
{
    using Hawk.Entities.Transaction.Payment;

    public class CurrencyMapping
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