namespace Hawk.Domain.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IDeleteStore
    {
        Task<Try<Unit>> Execute(Email email, string name);
    }
}
