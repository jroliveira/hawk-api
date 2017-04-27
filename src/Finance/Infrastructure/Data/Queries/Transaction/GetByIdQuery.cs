namespace Finance.Infrastructure.Data.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Collections;

    public class GetByIdQuery
    {
        public virtual async Task<Transaction> GetResultAsync(int id)
        {
            return await Task.Run(() => Transactions.Data.FirstOrDefault(transaction => transaction.Id == id));
        }
    }
}