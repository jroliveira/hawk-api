namespace Hawk.Domain.Commands.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;

    public interface ICreateCommand
    {
        Task<Account> Execute(Account entity);
    }
}
