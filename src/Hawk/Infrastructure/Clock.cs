namespace Hawk.Infrastructure
{
    using System;

    internal static class Clock
    {
        internal static Func<DateTime> Now { get; set; } = () => DateTime.Now;

        internal static Func<DateTime> UtcNow { get; set; } = () => DateTime.UtcNow;

        internal static void ResetClock()
        {
            Now = () => DateTime.Now;
            UtcNow = () => DateTime.UtcNow;
        }
    }
}
