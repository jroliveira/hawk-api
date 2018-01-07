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
            if (record == null || !record.Any())
            {
                return null;
            }

            var name = record.Get("name");
            var total = record.Get<int>("total");

            return new Store(name, total);
        }
    }
}