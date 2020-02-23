namespace Hawk.Infrastructure.Monad.Linq
{
    using System;

    using Hawk.Infrastructure.Monad;

    public static partial class LinqExtension
    {
        public static Try<TReturn> Select<TSuccess, TReturn>(this Try<TSuccess> @this, Func<TSuccess, TReturn> selector) => @this.Match<Try<TReturn>>(
            _ => _,
            success => selector(success));
    }
}
