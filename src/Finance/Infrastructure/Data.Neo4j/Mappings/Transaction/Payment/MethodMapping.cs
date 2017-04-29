namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction.Payment
{
    using Finance.Entities.Transaction.Payment;

    public class MethodMapping
    {
        public Method MapFrom(Record record)
        {
            if (record == null)
            {
                return null;
            }

            return new Method(
                record.Get("name"));
        }
    }
}
