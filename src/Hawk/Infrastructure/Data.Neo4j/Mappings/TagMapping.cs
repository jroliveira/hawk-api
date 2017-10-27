namespace Hawk.Infrastructure.Data.Neo4j.Mappings
{
    using Hawk.Entities.Transaction.Details;
    using Hawk.Infrastructure.Data.Neo4j.Extensions;

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