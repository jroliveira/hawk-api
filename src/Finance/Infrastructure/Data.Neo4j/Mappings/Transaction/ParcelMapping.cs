namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction
{
    using Finance.Entities.Transaction.Installment;

    public class ParcelMapping
    {
        public Parcel MapFrom(Record record)
        {
            if (record == null)
            {
                return null;
            }

            return new Parcel(
                record.Get<int>("total"),
                record.Get<int>("number"));
        }
    }
}