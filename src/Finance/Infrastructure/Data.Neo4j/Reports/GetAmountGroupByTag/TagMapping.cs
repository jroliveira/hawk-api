namespace Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupByTag
{
    using Finance.Infrastructure.Data.Neo4j.Extensions;
    using Finance.Reports.GetAmountGroupByTag;

    using global::Neo4j.Driver.V1;

    public class TagMapping
    {
        private readonly TransactionsMapping transactionsMapping;

        public TagMapping(TransactionsMapping transactionsMapping)
        {
            this.transactionsMapping = transactionsMapping;
        }

        public Tag MapFrom(IRecord record)
        {
            return this.MapFrom(record.GetRecord("data"));
        }

        public Tag MapFrom(Record record)
        {
            var transactions = this.transactionsMapping.MapFrom(record.GetRecord("transactions"));

            return new Tag
            {
                Name = record.Get("name"),
                Transactions = transactions
            };
        }
    }
}
