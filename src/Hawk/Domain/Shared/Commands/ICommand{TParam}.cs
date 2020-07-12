namespace Hawk.Domain.Shared.Commands
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    public interface ICommand<TParam>
        where TParam : Param
    {
        Task<Try<Unit>> Execute(Option<TParam> paramOption);
    }
}
