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

            Guard.NotNull(record, nameof(record), "Tag's record cannot be null.");

            return new Tag(
                record.Get("name"),
                record.Get<int>("total"));
        }
    }
}