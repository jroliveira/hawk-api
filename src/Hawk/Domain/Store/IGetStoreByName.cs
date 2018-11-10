namespace Hawk.Domain.Store
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface IGetStoreByName
    {
        Task<Try<Option<Store>>> GetResult(string name, string email);
    }
}
