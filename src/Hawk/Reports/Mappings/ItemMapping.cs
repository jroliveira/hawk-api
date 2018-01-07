namespace Hawk.Reports.Mappings
{
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Reports.Dtos;

    using Neo4j.Driver.V1;

    internal sealed class ItemMapping
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
