namespace Hawk.Infrastructure.Pagination
{
    using System;
    using System.Linq;

    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    public static class PageExtension
    {
        public static Try<Page<Try<TDestination>>> ToPage<TOrigin, TDestination>(
            this Page<Try<TOrigin>> @this,
            Func<TOrigin, TDestination> mapDestination) => new Page<Try<TDestination>>(
                @this
                    .Data
                    .Select(item => item.Match(
                        HandleException<TDestination>,
                        origin => mapDestination(origin))),
                @this.Skip,
                @this.Limit);
    }
}
