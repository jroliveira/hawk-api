namespace Hawk.Domain.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IUpsertStore
    {
        Task<Try<Store>> Execute(Email email, string name, Option<Store> entity);
    }
}
