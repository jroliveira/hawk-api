namespace Hawk.Domain.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IDeleteTransaction
    {
        Task<Try<Unit>> Execute(Email email, Guid id);
    }
}
