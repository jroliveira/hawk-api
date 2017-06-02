namespace Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupBy
{
    using Finance.Reports.GetAmountGroupBy;

    public class TransactionsMapping
    {
        public Transactions MapFrom(Record record)
        {
            return new Transactions
            {
                Period = record.Get("period"),
                Amount = record.Get<double>("amount"),
                Quantity = record.Get<int>("quantity")
            };
        }
    }
}