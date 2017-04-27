namespace Finance.Infrastructure.Data.Queries.Account
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities;
    using Finance.Infrastructure.Data.Collections;

    public class GetByEmailQuery
    {
        public virtual async Task<Account> GetResultAsync(string email)
        {
            return await Task.Run(() => Accounts.Data.FirstOrDefault(account => account.Email == email));
        }
    }
}