namespace Hawk.Infrastructure.Logging
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    public sealed class Logger
    {
        private static Logger? logger;

        private readonly Action<LogLevel, string> logMethod;

        private Logger(LogLevel level, Action<LogLevel, string> logMethod)
        {
            this.logMethod = logMethod;
            this.Level = level;
        }

        public LogLevel Level { get; }

        public static void NewLogger(
            LogLevel level,
            Action<LogLevel, string> logMethod) => logger = new Logger(level, logMethod);

        public static void LogError<TModel>(string message, object data, TryModel<TModel> tryModel) => logger?.Log(
            LogLevel.Error,
            message,
            new { Info = data, Error = tryModel });

        public static void LogError(string message, Exception exception) => LogError<Unit>(message, exception);

        public static void LogError<TModel>(string message, Exception exception) => logger?.Log(
            LogLevel.Error,
            message,
            new { Error = HandleException<TModel>(exception, true) });

        public static void LogError(string message, object data) => logger?.Log(
            LogLevel.Error,
            message,
            new { Info = data });

        public static void LogInfo(string message, object data) => logger?.Log(
            LogLevel.Info,
            new DefaultLogData(
                LogLevel.Info,
                message,
                new { Info = data }));

        public static void LogWarning(string message, object data) => logger?.Log(
            LogLevel.Warn,
            new DefaultLogData(
                LogLevel.Warn,
                message,
                new { Info = data }));

        private void Log(LogLevel level, string message, object data) => this.Log(
            level,
            new DefaultLogData(
                level,
                message,
                data));

        private void Log(LogLevel level, ILogData data)
        {
            if (this.Level < level || level == LogLevel.Verb)
            {
                return;
            }

            this.logMethod(level, data.ToString());
        }
    }
}
