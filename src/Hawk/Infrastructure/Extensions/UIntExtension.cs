namespace Hawk.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;

    using static System.Collections.Immutable.ImmutableList;

    public static class UIntExtension
    {
        public static IEnumerable<TReturn> ForEach<TReturn>(this uint until, uint starting, Func<uint, TReturn> addItem)
        {
            var items = Create<TReturn>();

            for (uint i = 1; i <= until - starting; i++)
            {
                items = items.Add(addItem(i));
            }

            return items;
        }
    }
}
