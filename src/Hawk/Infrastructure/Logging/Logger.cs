namespace Hawk.Infrastructure.Logging
{
    using System;

    public sealed class Logger
    {
        private readonly Action<string> logMethod;

        public Logger(LogLevel level, Action<string> logMethod)
        {
            this.logMethod = logMethod;
            this.Level = level;
        }

        public LogLevel Level { get; }

        public void Error(ILogData data)
        {
            this.Log(LogLevel.Error, data);
        }

        public void Info(ILogData data)
        {
            this.Log(LogLevel.Info, data);
        }

        private void Log(LogLevel level, ILogData data)
        {
            if (this.Level < level || level == LogLevel.None)
            {
                return;
            }

            this.logMethod(data.ToString());
        }
    }
}
