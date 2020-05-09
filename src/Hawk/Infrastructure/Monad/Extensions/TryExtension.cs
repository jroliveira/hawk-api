namespace Hawk.Infrastructure.Monad.Extensions
{
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public static class TryExtension
    {
        public static string? GetStringOrElse<TSuccess>(this Try<TSuccess> @this, string @default) => @this.Match(
            _ => @default,
            value => value?.ToString());

        public static Option<TSuccess> ToOption<TSuccess>(this Try<TSuccess> @this) => @this.Match(
            _ => None(),
            Some);
    }
}
