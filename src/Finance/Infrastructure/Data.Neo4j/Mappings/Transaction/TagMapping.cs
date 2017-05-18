namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction
{
    using Finance.Entities.Transaction.Details;
    using Finance.Infrastructure.Data.Neo4j.Extensions;

    using global::Neo4j.Driver.V1;

    public class TagMapping : IMapping<Tag>
    {
        public Tag MapFrom(IRecord record)
        {
            return this.MapFrom(record.GetRecord("data"));
        }

        public Tag MapFrom(Record record)
        {
            var name = record.Get("name");
            return new Tag(name)
            {
                Total = record.Get<int>("total")
            };
        }
    }
}