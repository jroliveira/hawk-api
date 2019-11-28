namespace Hawk.Infrastructure.Logging
{
    using System;

    public sealed class Logger
    {
        private static Logger? logger;

        private readonly Func<string> tracking;
        private readonly Action<string> logMethod;

        private Logger(LogLevel level, Func<string> tracking, Action<string> logMethod)
        {
            this.tracking = tracking;
            this.logMethod = logMethod;
            this.Level = level;

            logger = this;
        }

        public LogLevel Level { get; }

        public static void NewLogger(
            LogLevel level,
            Func<string> tracking,
            Action<string> logMethod) => logger = new Logger(level, tracking, logMethod);

        public static void LogError(string message, object data) => logger?.Log(
            LogLevel.Error,
            new DefaultLogData(
                LogLevel.Error,
                message,
                logger.tracking(),
                data));

        public static void LogInfo(string message, object data) => logger?.Log(
            LogLevel.Info,
            new DefaultLogData(
                LogLevel.Info,
                message,
                logger.tracking(),
                data));

        public static void LogWarning(string message, object data) => logger?.Log(
            LogLevel.Warn,
            new DefaultLogData(
                LogLevel.Warn,
                message,
                logger.tracking(),
                data));

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
