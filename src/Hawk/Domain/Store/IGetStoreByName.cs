namespace Hawk.Domain.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IGetStoreByName
    {
        Task<Try<Store>> GetResult(Email email, string name);
    }
}
