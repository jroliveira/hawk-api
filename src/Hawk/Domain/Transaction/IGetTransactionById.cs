namespace Hawk.Domain.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetTransactionById
    {
        Task<Try<Transaction>> GetResult(Option<Email> email, Option<Guid> id);
    }
}
