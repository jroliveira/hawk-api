namespace Hawk.Domain.Commands.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;

    public interface ICreateCommand
    {
        Task<Try<Store>> Execute(Store entity);
    }
}
