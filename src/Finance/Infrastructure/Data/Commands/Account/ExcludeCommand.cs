namespace Finance.Infrastructure.Data.Commands.Account
{
    using System.Threading.Tasks;

    using Finance.Entities;

    public class ExcludeCommand
    {
        public virtual async Task ExecuteAsync(Account entity)
        {
            await Task.Run(() => entity.MarkAsDeleted());
        }
    }
}