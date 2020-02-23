namespace Hawk.Domain.Shared.Commands
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public abstract class Command<TParam> : ICommand<TParam>
        where TParam : Param
    {
        public Task<Try<Unit>> Execute(Option<TParam> param) => param.Match(
            this.Execute,
            () => Task(Failure<Unit>(new NullObjectException("Param is required."))));

        protected abstract Task<Try<Unit>> Execute(TParam param);
    }
}
