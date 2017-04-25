namespace Finance.Infrastructure.Data.Commands.Account
{
    using System.Threading.Tasks;

    using Finance.Entities;

    public class CreateCommand
    {
        public virtual async Task<Account> ExecuteAsync(Account entity)
        {
            entity.HashPassword();

            return await Task.Run(() => entity);
        }
    }
}