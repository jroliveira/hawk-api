namespace Hawk.Domain.Queries.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;

    public interface IGetByEmailQuery
    {
        Task<Account> GetResult(string email);
    }
}
