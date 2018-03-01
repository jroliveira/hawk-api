namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;

    using Neo4j.Driver.V1;

    internal sealed class StoreMapping
    {
        public (Store Store, int Count) MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
        }

        public (Store Store, int Count) MapFrom(Record record)
        {
            Guard.NotNull(record, nameof(record), "Store's record cannot be null.");

            var store = new Store(record.Get("name"));

            foreach (var tag in record.GetList("tags"))
            {
                store.AddTag(new Tag(tag));
            }

            return (store, record.Get<int>("total"));
        }
    }
}