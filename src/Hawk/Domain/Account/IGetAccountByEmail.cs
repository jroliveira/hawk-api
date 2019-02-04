namespace Hawk.Domain.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetAccountByEmail
    {
        Task<Try<Account>> GetResult(Email email);
    }
}
