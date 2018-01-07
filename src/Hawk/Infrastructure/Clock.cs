namespace Hawk.Infrastructure
{
    using System;

    internal static class Clock
    {
        public static Func<DateTime> Now { get; set; } = () => DateTime.Now;

        public static Func<DateTime> UtcNow { get; set; } = () => DateTime.UtcNow;

        public static void ResetClock()
        {
            Now = () => DateTime.Now;
            UtcNow = () => DateTime.UtcNow;
        }
    }
}
