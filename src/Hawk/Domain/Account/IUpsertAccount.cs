namespace Hawk.Domain.Account
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface IUpsertAccount
    {
        Task<Try<Account>> Execute(Account entity);
    }
}
