namespace Hawk.Infrastructure.Data.Neo4j.Reports.GetAmountGroupBy
{
    using Hawk.Entities.Transaction;

    public class Item
    {
        public string Name { get; set; }

        public Transactions Transactions { get; set; }
    }
}
