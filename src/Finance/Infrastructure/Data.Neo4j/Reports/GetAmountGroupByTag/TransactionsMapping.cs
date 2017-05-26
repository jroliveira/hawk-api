namespace Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupByTag
{
    using Finance.Reports.GetAmountGroupByTag;

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