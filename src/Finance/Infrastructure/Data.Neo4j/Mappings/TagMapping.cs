namespace Finance.Infrastructure.Data.Neo4j.Mappings
{
    using Finance.Entities.Transaction.Details;
    using Finance.Infrastructure.Data.Neo4j.Extensions;

    using global::Neo4j.Driver.V1;

    public class TagMapping
    {
        public Tag MapFrom(IRecord data)
        {
            var record = data.GetRecord("data");
            if (record == null || !record.Any())
            {
                return null;
            }

            var name = record.Get("name");
            return new Tag(name)
            {
                Total = record.Get<int>("total")
            };
        }
    }
}