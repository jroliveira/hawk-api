namespace Hawk.WebApi.Infrastructure.Logging.Http
{
    using System;
    using System.Diagnostics;

    public sealed class Elapse
    {
        internal Elapse(Stopwatch stopwatch)
        {
            this.TimeSpan = stopwatch.Elapsed;
            this.Milliseconds = stopwatch.Elapsed.Milliseconds;
        }

        public TimeSpan TimeSpan { get; }

        public int Milliseconds { get; }
    }
}
