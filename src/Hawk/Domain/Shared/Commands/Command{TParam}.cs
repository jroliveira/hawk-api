namespace Hawk.Domain.Shared.Commands
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public abstract class Command<TParam> : ICommand<TParam>
        where TParam : Param
    {
        public Task<Try<Unit>> Execute(Option<TParam> paramOption) => paramOption
            .Fold(Task(Failure<Unit>(IsRequired(nameof(Param)))))(this.Execute);

        protected abstract Task<Try<Unit>> Execute(TParam param);
    }
}
