namespace Hawk.WebApi.Infrastructure.Logging.Http
{
    using Microsoft.AspNetCore.Http;

    public sealed class Request
    {
        internal Request(HttpContext context)
        {
            this.ContentType = context.Request.ContentType;
            this.Headers = context.Request.Headers;
            this.Method = context.Request.Method;
        }

        public string ContentType { get; }

        public IHeaderDictionary Headers { get; }

        public string Method { get; }
    }
}
