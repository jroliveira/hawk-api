namespace Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupBy
{
    using Finance.Infrastructure.Data.Neo4j.Extensions;
    using Finance.Reports.GetAmountGroupBy;

    using global::Neo4j.Driver.V1;

    public class ItemMapping
    {
        private readonly TransactionsMapping transactionsMapping;

        public ItemMapping(TransactionsMapping transactionsMapping)
        {
            this.transactionsMapping = transactionsMapping;
        }

        public Item MapFrom(IRecord record)
        {
            return this.MapFrom(record.GetRecord("data"));
        }

        public Item MapFrom(Record record)
        {
            var transactions = this.transactionsMapping.MapFrom(record.GetRecord("transactions"));

            return new Item
            {
                Name = record.Get("name"),
                Transactions = transactions
            };
        }
    }
}
