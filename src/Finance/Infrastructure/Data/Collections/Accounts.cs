namespace Finance.Infrastructure.Data.Collections
{
    using System.Collections.ObjectModel;

    using Finance.Entities;

    public class Accounts : Collection<Account>
    {
        public Accounts()
        {
            this.Add(new Account(1, "junolive@gmail.com", "123456"));
        }

        public static Accounts Data => new Accounts();
    }
}
