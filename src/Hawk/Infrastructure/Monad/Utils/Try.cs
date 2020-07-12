namespace Hawk.Infrastructure.Monad.Utils
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    public static partial class Util
    {
        public static Try<TSuccess> Success<TSuccess>(in TSuccess success) => new Try<TSuccess>(success);

        public static Try<TValue> Failure<TValue>(in BaseException exception) => new Try<TValue>(exception);
    }
}
