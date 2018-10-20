namespace Hawk.Domain.Queries.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;

    public interface IGetByEmailQuery
    {
        Task<Try<Option<Account>>> GetResult(string email);
    }
}
