namespace Finance.Infrastructure.Data.Neo4j.Commands.Transaction
{
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;

    public class ExcludeCommand
    {
        public virtual async Task ExecuteAsync(Transaction entity)
        {
            await Task.Run(() => entity.MarkAsDeleted());
        }
    }
}