namespace Hawk.Domain.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface IDeleteTransaction
    {
        Task<Try<Unit>> Execute(Transaction entity);
    }
}
