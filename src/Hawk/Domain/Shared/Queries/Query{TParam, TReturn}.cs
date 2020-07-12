namespace Hawk.Domain.Shared.Queries
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public abstract class Query<TParam, TReturn> : IQuery<TParam, TReturn>
        where TParam : Param
    {
        public Task<Try<TReturn>> GetResult(Option<TParam> paramOption) => paramOption
            .Fold(Task(Failure<TReturn>(IsRequired(nameof(Param)))))(this.GetResult);

        protected abstract Task<Try<TReturn>> GetResult(TParam param);
    }
}
