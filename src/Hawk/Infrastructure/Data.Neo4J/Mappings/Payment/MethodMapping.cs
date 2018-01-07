namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;

    using Neo4j.Driver.V1;

    internal sealed class MethodMapping
    {
        public Method MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
        }

        public Method MapFrom(Record record)
        {
            if (record == null || !record.Any())
            {
                return null;
            }

            var name = record.Get("name");
            var total = record.Get<int>("total");

            return new Method(name, total);
        }
    }
}
