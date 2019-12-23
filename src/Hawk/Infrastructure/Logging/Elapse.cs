namespace Hawk.Infrastructure.Logging
{
    using System;
    using System.Diagnostics;

    public sealed class Elapse
    {
        private Elapse(TimeSpan timeSpan) => this.TimeSpan = timeSpan;

        public TimeSpan TimeSpan { get; }

        public static implicit operator Elapse(Stopwatch elapse) => new Elapse(elapse.Elapsed);
    }
}
