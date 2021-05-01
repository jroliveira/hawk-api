namespace Hawk.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;

    using static System.Collections.Immutable.ImmutableList;

    public static class UIntExtension
    {
        public static IEnumerable<TReturn> ForEach<TReturn>(this uint until, uint starting, Func<uint, uint, TReturn> addItem)
        {
            var items = Create<TReturn>();
            var index = 0u;

            for (var current = starting; current <= until; current++)
            {
                index++;
                items = items.Add(addItem(current, index));
            }

            return items;
        }
    }
}
