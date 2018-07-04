namespace Hawk.Domain.Queries.Store
{
    using System.Threading.Tasks;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;

    public interface IGetByNameQuery
    {
        Task<Try<Option<Store>>> GetResult(string name, string email);
    }
}