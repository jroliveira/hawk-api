namespace Hawk.WebApi.Infrastructure.Logging.Http
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Http;

    public sealed class LogData
    {
        internal LogData(string message, Stopwatch stopwatch, HttpContext context)
        {
            this.Message = message;
            this.Elapse = new Elapse(stopwatch);
            this.Request = new Request(context);
            this.Response = new Response(context);
        }

        public string Message { get; }

        public Elapse Elapse { get; }

        public Request Request { get; }

        public Response Response { get; }
    }
}
