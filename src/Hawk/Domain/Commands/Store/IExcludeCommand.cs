namespace Hawk.Domain.Commands.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;

    public interface IExcludeCommand
    {
        Task<Try<Unit>> Execute(Store entity);
    }
}
