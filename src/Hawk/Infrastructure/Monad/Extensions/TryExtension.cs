namespace Hawk.Infrastructure.Monad.Extensions
{
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public static class TryExtension
    {
        public static TSuccess GetOrElse<TSuccess>(this Try<TSuccess> @this, TSuccess @default) => @this.Match(
            _ => @default,
            value => value);

        public static Option<TSuccess> ToOption<TSuccess>(this Try<TSuccess> @this) => @this.Match(
            _ => None,
            Some);

        public static Try<TSuccess> Lift<TSuccess>(this Try<Try<TSuccess>> @this) => @this.Match(
            failure => failure,
            success => success);
    }
}
