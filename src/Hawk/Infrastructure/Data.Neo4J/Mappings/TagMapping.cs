namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;

    using Neo4j.Driver.V1;

    internal sealed class TagMapping
    {
        public (Tag Tag, int Count) MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
        }

        public (Tag Tag, int Count) MapFrom(Record record)
        {
            Guard.NotNull(record, nameof(record), "Tag's record cannot be null.");

            var tag = new Tag(record.Get("name"));

            return (tag, record.Get<int>("total"));
        }
    }
}