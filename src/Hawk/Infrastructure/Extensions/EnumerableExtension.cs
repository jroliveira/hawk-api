namespace Hawk.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;

    internal static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
            {
                action(item);
            }
        }
    }
}
