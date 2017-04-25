namespace Finance.Infrastructure.Data.Commands.Transaction
{
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;

    public class CreateCommand
    {
        public virtual async Task<Transaction> ExecuteAsync(Transaction entity)
        {
            return await Task.Run(() => entity);
        }
    }
}