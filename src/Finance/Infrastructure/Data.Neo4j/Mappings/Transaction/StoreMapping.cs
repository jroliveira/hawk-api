namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction
{
    using Finance.Entities.Transaction.Details;

    public class StoreMapping
    {
        public Store MapFrom(Record record)
        {
            if (record == null || !record.Any())
            {
                return null;
            }

            return new Store(
                record.Get("name"));
        }
    }
}