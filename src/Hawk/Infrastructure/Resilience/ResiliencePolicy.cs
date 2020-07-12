namespace Hawk.Infrastructure.Resilience
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Resilience.Configurations;

    using Microsoft.Extensions.Options;

    using Polly;
    using Polly.CircuitBreaker;
    using Polly.Retry;

    using static System.Threading.Tasks.Task;
    using static System.TimeSpan;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;
    using static Hawk.Infrastructure.Logging.Logger;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static Polly.Policy;

    internal sealed class ResiliencePolicy
    {
        private readonly AsyncRetryPolicy? policy;

        public ResiliencePolicy(IOptions<ResilienceConfiguration> config)
        {
            if (!config.Value.IsEnabled())
            {
                return;
            }

            var (_, (retryCount, retryTimeInMs)) = config.Value;
            var sleepDurationProvider = GetSleepDurationProvider(retryTimeInMs);

            Handle<Exception>()
                .WaitAndRetry(retryCount, sleepDurationProvider);

            this.policy = Handle<BrokenCircuitException>()
                .Or<Exception>()
                .WaitAndRetryAsync(retryCount, sleepDurationProvider, WriteLog);
        }

        internal Task Execute(in Func<Context?, Task> func) => this.Execute(new Dictionary<string, object>(), func);

        internal Task Execute(in IDictionary<string, object> contextData, Func<Context?, Task> func) => this.Execute(contextData, _ =>
        {
            func(_);
            return FromResult(Unit());
        });

        internal Task<TResult> Execute<TResult>(in Func<Context?, Task<TResult>> func) => this.Execute(new Dictionary<string, object>(), func);

        internal Task<TResult> Execute<TResult>(in IDictionary<string, object> contextData, in Func<Context?, Task<TResult>> func) => this.policy == default
            ? func(default)
            : this.policy.ExecuteAsync(func, contextData);

        private static Func<int, TimeSpan> GetSleepDurationProvider(int timeInMs) => count => FromMilliseconds(timeInMs * count);

        private static Task WriteLog(
            Exception exception,
            TimeSpan timeSpan,
            int retryCount,
            Context context)
        {
            var logInfo = new { RetryCount = retryCount, context.PolicyKey };

            if (exception == default)
            {
                LogError("A non success operation was received on retry count for policy key.", logInfo);
            }
            else
            {
                LogError("An exception occurred on retry count for policy key.", logInfo, HandleException(exception));
            }

            return CompletedTask;
        }
    }
}
