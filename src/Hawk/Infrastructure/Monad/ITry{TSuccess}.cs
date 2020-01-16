namespace Hawk.Infrastructure.Monad
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    public interface ITry<out TSuccess>
    {
        TSuccess Get();

        TReturn Match<TReturn>(Func<BaseException, TReturn> failure, Func<TSuccess, TReturn> success);
    }
}
