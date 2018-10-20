namespace Hawk.Domain.Queries.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;

    public interface IGetByIdQuery
    {
        Task<Try<Option<Transaction>>> GetResult(string id, string email);
    }
}
