namespace Finance.Infrastructure.Data.Neo4j.Mappings.Payment
{
    using Finance.Entities.Transaction.Payment;
    using Finance.Infrastructure.Data.Neo4j.Extensions;

    using global::Neo4j.Driver.V1;

    public class MethodMapping : IMapping<Method>
    {
        public Method MapFrom(IRecord record)
        {
            return this.MapFrom(record.GetRecord("data"));
        }

        public Method MapFrom(Record record)
        {
            if (record == null || !record.Any())
            {
                return null;
            }

            var name = record.Get("name");
            return new Method(name)
            {
                Total = record.Get<int>("total")
            };
        }
    }
}
