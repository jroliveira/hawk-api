namespace Hawk.WebApi.Infrastructure.Logging.Http
{
    using Microsoft.AspNetCore.Http;

    public sealed class Response
    {
        internal Response(HttpContext context)
        {
            this.ContentType = context.Response.ContentType;
            this.Headers = context.Response.Headers;
            this.StatusCode = context.Response.StatusCode;
        }

        public string ContentType { get; }

        public IHeaderDictionary Headers { get; }

        public int StatusCode { get; }
    }
}
