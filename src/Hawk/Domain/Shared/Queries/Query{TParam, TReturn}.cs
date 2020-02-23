namespace Hawk.Domain.Shared.Queries
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public abstract class Query<TParam, TReturn> : IQuery<TParam, TReturn>
        where TParam : Param
    {
        public Task<Try<TReturn>> GetResult(Option<TParam> param) => param.Match(
            this.GetResult,
            () => Task(Failure<TReturn>(new NullObjectException("Param is required."))));

        protected abstract Task<Try<TReturn>> GetResult(TParam param);
    }
}
