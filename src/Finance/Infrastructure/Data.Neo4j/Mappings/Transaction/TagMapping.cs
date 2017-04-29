namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction
{
    using Finance.Entities.Transaction.Details;

    public class TagMapping
    {
        public Tag MapFrom(Record record)
        {
            if (record == null)
            {
                return null;
            }

            return new Tag(
                record.Get("name"));
        }
    }
}