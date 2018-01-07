namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;

    using Neo4j.Driver.V1;

    internal sealed class TagMapping
    {
        public Tag MapFrom(IRecord data)
        {
            var record = data.GetRecord("data");
            if (record == null || !record.Any())
            {
                return null;
            }

            var name = record.Get("name");
            var total = record.Get<int>("total");

            return new Tag(name, total);
        }
    }
}