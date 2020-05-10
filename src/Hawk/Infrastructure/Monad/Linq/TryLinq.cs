namespace Hawk.Infrastructure.Monad.Linq
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public static partial class LinqExtension
    {
        public static Try<TReturn> Select<TSuccess, TReturn>(this Try<TSuccess> @this, Func<TSuccess, TReturn> selector) => @this.Match<Try<TReturn>>(
            _ => _,
            success => selector(success));

        public static Task<Try<TReturn>> Select<TSuccess, TReturn>(this Try<TSuccess> @this, Func<TSuccess, Task<Try<TReturn>>> selector) => @this.Match(
            _ => Task(Failure<TReturn>(_)),
            selector);
    }
}
