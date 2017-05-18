namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction
{
    using Finance.Entities.Transaction.Details;
    using Finance.Infrastructure.Data.Neo4j.Extensions;

    using global::Neo4j.Driver.V1;

    public class StoreMapping : IMapping<Store>
    {
        public Store MapFrom(IRecord record)
        {
            return this.MapFrom(record.GetRecord("data"));
        }

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