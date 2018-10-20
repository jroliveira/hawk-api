namespace Hawk.Domain.Commands.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;

    public interface ICreateCommand
    {
        Task<Try<Transaction>> Execute(Transaction entity);
    }
}
