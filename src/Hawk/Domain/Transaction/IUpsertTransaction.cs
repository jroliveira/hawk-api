namespace Hawk.Domain.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface IUpsertTransaction
    {
        Task<Try<Transaction>> Execute(Transaction entity);
    }
}
