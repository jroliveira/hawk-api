namespace Hawk.Domain.Account
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface IUpsertAccount
    {
        Task<Try<Account>> Execute(Option<Account> entity);
    }
}
