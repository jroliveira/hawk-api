namespace Hawk.Infrastructure.Logging
{
    using System;

    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    public sealed class Logger
    {
        private static Logger? logger;

        private readonly Action<LogLevel, string> logMethod;

        private Logger(in LogLevel level, in Action<LogLevel, string> logMethod)
        {
            this.logMethod = logMethod;
            this.Level = level;
        }

        public LogLevel Level { get; }

        public static void NewLogger(in LogLevel level, in Action<LogLevel, string> logMethod) => logger = new Logger(level, logMethod);

        public static void LogError<TModel>(
            in string message,
            in object data,
            in Try<TModel> tryModel) => logger?.Log(
            LogLevel.Error,
            message,
            new { Info = data, Error = tryModel });

        public static void LogError(in string message) => logger?.Log(
            LogLevel.Error,
            message,
            new { });

        public static void LogError(in string message, in Exception exception) => LogError<Unit>(message, exception);

        public static void LogError<TModel>(in string message, in Exception exception) => logger?.Log(
            LogLevel.Error,
            message,
            new { Error = HandleException<TModel>(exception, true) });

        public static void LogError(in string message, in object data) => logger?.Log(
            LogLevel.Error,
            message,
            new { Info = data });

        public static void LogInfo(in string message, in object data) => logger?.Log(
            LogLevel.Info,
            new DefaultLogData(
                LogLevel.Info,
                message,
                new { Info = data }));

        public static void LogWarning<TModel>(in string message, in Exception exception) => logger?.Log(
            LogLevel.Warn,
            message,
            new { Info = HandleException<TModel>(exception, true) });

        public static void LogWarning(in string message, in object data) => logger?.Log(
            LogLevel.Warn,
            new DefaultLogData(
                LogLevel.Warn,
                message,
                new { Info = data }));

        private void Log(
            in LogLevel level,
            in string message,
            in object data) => this.Log(
            level,
            new DefaultLogData(
                level,
                message,
                data));

        private void Log(in LogLevel level, in ILogData data)
        {
            if (this.Level < level || level == LogLevel.Verb)
            {
                return;
            }

            this.logMethod(level, data.ToString());
        }
    }
}
