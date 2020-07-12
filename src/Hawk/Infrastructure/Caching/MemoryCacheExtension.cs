namespace Hawk.Infrastructure.Caching
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.Caching.Memory;

    using static System.TimeSpan;

    public static class MemoryCacheExtension
    {
        private static readonly Random Random = new Random(1);

        public static Task<Try<TOutput>> GetOrCreateCache<TInput, TOutput>(
            this IMemoryCache @this,
            in TInput input,
            Func<Task<Try<TOutput>>> func) => @this.GetOrCreateAsync(
                input,
                async entry => await func() switch
                {
                    var @try when @try => RenewCache(entry, @try),
                    var @try => ExpireCache(entry, @try),
                });

        private static Try<TOutput> RenewCache<TOutput>(in ICacheEntry entry, in Try<TOutput> @try)
        {
            entry.SetSlidingExpiration(FromMinutes(5 + Random.Next(1, 5)));
            return @try;
        }

        private static Try<TOutput> ExpireCache<TOutput>(in ICacheEntry entry, in Try<TOutput> @try)
        {
            entry.SetAbsoluteExpiration(FromMilliseconds(1));
            return @try;
        }
    }
}
