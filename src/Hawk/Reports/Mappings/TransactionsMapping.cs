namespace Hawk.Reports.Mappings
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Reports.Dtos;

    internal sealed class TransactionsMapping
    {
        public Transactions MapFrom(Record record)
        {
            if (record == null || !record.Any())
            {
                return null;
            }

            var types = new Dictionary<string, string>
            {
                { "Debit", "Debit" },
                { "Credit", "Credit" }
            };

            var type = record
                .GetList("type")
                .FirstOrDefault(t => types.ContainsKey(t.ToString()));

            return new Transactions
            {
                Period = record.Get("period"),
                Amount = record.Get<double>("amount"),
                Quantity = record.Get<int>("quantity"),
                Type = type
            };
        }
    }
}