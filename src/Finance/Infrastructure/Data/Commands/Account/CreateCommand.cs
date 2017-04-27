namespace Finance.Infrastructure.Data.Commands.Account
{
    using System.Threading.Tasks;

    using Finance.Entities;
    using Finance.Infrastructure.Data.Collections;

    public class CreateCommand
    {
        public virtual async Task<Account> ExecuteAsync(Account entity)
        {
            entity.HashPassword();

            Accounts.Data.Add(entity);

            return await Task.Run(() => entity);
        }
    }
}