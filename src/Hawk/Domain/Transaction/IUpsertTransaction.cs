namespace Hawk.Domain.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertTransaction
    {
        Task<Try<Transaction>> Execute(Email email, Option<Transaction> entity);
    }
}
