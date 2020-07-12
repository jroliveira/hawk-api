namespace System.Linq
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

        public static Try<TReturn> Select<TSuccess, TReturn>(this Try<TSuccess> @this, Func<TSuccess, Try<TReturn>> selector) => @this.Match(
            _ => _,
            selector);

        public static Task<Try<TReturn>> Select<TSuccess, TReturn>(this Try<TSuccess> @this, Func<TSuccess, Task<Try<TReturn>>> selector) => @this.Match(
            _ => Task(Failure<TReturn>(_)),
            selector);

        public static Func<Func<TSuccess, TReturn>, TReturn> Fold<TSuccess, TReturn>(this Try<TSuccess> @this, TReturn ifEmpty) => selector => @this.Match(
            _ => ifEmpty,
            selector);

        public static Unit ForEach<TSuccess>(this Try<TSuccess> @this, Action<TSuccess> selector) => @this.Match(
            _ => Unit(),
            success =>
            {
                selector(success);
                return Unit();
            });
    }
}
