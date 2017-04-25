namespace Finance.Infrastructure.Data.Queries.Account
{
    using System.Threading.Tasks;

    using Finance.Entities;

    public class GetByEmailQuery
    {
        public virtual async Task<Account> GetResultAsync(string email)
        {
            return await Task.Run(() => new Account("junolive@gmail.com", "123456"));
        }
    }
}