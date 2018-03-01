namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;

    using Neo4j.Driver.V1;

    internal sealed class MethodMapping
    {
        public (Method Method, int Count) MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
        }

        public (Method Method, int Count) MapFrom(Record record)
        {
            Guard.NotNull(record, nameof(record), "Payment method's record cannot be null.");

            return (
                new Method(record.Get("name")),
                record.Get<int>("total"));
        }
    }
}
