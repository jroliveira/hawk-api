namespace Hawk.Infrastructure.Logging
{
    using System;
    using System.Text;

    public sealed class Logger
    {
        private static Logger logger;

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

        public static void LogError(string message, Exception exception = null)
        {
            var logMessage = new StringBuilder(message);

            if (exception != null)
            {
                logMessage.Append($"Exception: {exception.Message}.");

                var innerException = exception.InnerException;
                while (innerException != null)
                {
                    logMessage.Append($" Inner exception: {innerException.Message}");
                    innerException = innerException.InnerException;
                }
            }

            logger.Log(LogLevel.Error, new DefaultLogData(logger.Level, logger.tracking(), logMessage.ToString()));
        }

        public static void LogInfo(object data) => logger.Log(LogLevel.Info, new DefaultLogData(LogLevel.Info, logger.tracking(), data));

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
