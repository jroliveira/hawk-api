namespace Hawk.Domain.Commands.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;

    public interface ICreateCommand
    {
        Task<Try<Account>> Execute(Account entity);
    }
}
