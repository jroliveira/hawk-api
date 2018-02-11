namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;

    using Neo4j.Driver.V1;

    internal sealed class StoreMapping
    {
        public Store MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
        }

        public Store MapFrom(Record record)
        {
            Guard.NotNull(record, nameof(record), "Store's record cannot be null.");

            return new Store(
                record.Get("name"),
                record.Get<int>("total"));
        }
    }
}