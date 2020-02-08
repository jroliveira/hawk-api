namespace Hawk.Test.Infrastructure.Monad.Extensions
{
    using Hawk.Infrastructure.Monad;

    internal static class TryExtension
    {
        internal static TryAssertions<TSuccess> Must<TSuccess>(this Try<TSuccess> @this) => @this;
    }
}
