namespace Hawk.Domain.Account
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface IGetAccountByEmail
    {
        Task<Try<Option<Account>>> GetResult(string email);
    }
}
