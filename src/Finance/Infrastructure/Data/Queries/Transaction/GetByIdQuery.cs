namespace Finance.Infrastructure.Data.Queries.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Finance.Entities;
    using Finance.Entities.Transaction;

    public class GetByIdQuery
    {
        public virtual async Task<Transaction> GetResultAsync(int id)
        {
            return await Task.Run(() => new Debit(10, DateTime.Now, new Account("junolive@gmail.com", "123456")));
        }
    }
}