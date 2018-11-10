namespace Hawk.Domain.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface IGetTransactionById
    {
        Task<Try<Option<Transaction>>> GetResult(string id, string email);
    }
}
