namespace Hawk.WebApi.Infrastructure.Logging.Http
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using static Hawk.Infrastructure.Guard;
    using static Hawk.Infrastructure.Logging.Logger;

    internal sealed class LoggingHttpMiddleware
    {
        private readonly RequestDelegate next;

        public LoggingHttpMiddleware(RequestDelegate next)
        {
            NotNull(next, nameof(next), "Http log middleware's next cannot be null.");

            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await this.next(context);

            stopwatch.Stop();

            LogInfo(new LogData("Time spent during request execution.", stopwatch, context));
        }
    }
}
