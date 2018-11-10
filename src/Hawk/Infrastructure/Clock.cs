namespace Hawk.Infrastructure
{
    using System;

    internal static class Clock
    {
        internal static Func<DateTime> UtcNow { get; set; } = () => DateTime.UtcNow;
    }
}
