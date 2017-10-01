namespace Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupBy
{
    using System.Collections.Generic;
    using System.Linq;

    using Finance.Reports.GetAmountGroupBy;

    public class TransactionsMapping
    {
        public Transactions MapFrom(Record record)
        {
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