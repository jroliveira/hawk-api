namespace Finance.Infrastructure.Data.Commands.Transaction
{
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Collections;

    public class CreateCommand
    {
        public virtual async Task<Transaction> ExecuteAsync(Transaction entity)
        {
            Transactions.Data.Add(entity);
            return await Task.Run(() => entity);
        }
    }
}