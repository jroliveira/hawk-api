namespace Hawk.Infrastructure.Monad.Extensions
{
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public static class TryExtension
    {
        public static Option<TSuccess> ToOption<TSuccess>(this Try<TSuccess> @this) => @this.Match(
            _ => None(),
            Some);
    }
}
