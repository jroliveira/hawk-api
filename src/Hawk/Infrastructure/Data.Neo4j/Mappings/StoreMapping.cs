namespace Hawk.Infrastructure.Data.Neo4j.Mappings
{
    using Hawk.Entities.Transaction.Details;
    using Hawk.Infrastructure.Data.Neo4j.Extensions;

    using global::Neo4j.Driver.V1;

    public class StoreMapping
    {
        private readonly TransactionsMapping transactionsMapping;

        public StoreMapping(TransactionsMapping transactionsMapping)
        {
            this.transactionsMapping = transactionsMapping;
        }

        public Store MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
        }

        public Store MapFrom(Record record)
        {
            if (record == null || !record.Any())
            {
                return null;
            }

            var name = record.Get("name");
            var transactions = this.transactionsMapping.MapFrom(record.GetRecord("transactions"));

            return new Store(name)
            {
                Total = record.Get<int>("total"),
                Transactions = transactions
            };
        }
    }
}