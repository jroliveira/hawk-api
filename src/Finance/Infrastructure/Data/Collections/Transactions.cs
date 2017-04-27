namespace Finance.Infrastructure.Data.Collections
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Finance.Entities.Transaction;

    public class Transactions : Collection<Transaction>
    {
        public Transactions()
        {
            this.Add(new Credit(1, 2000, DateTime.Now, Accounts.Data.First()));
            this.Add(new Debit(2, 10, DateTime.Now, Accounts.Data.First()));
        }

        public static Transactions Data => new Transactions();
    }
}