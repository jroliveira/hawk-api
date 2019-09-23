namespace Hawk.WebApi.Infrastructure.Hal.Link
{
    using System.Net.Http;

    internal sealed class Link
    {
        internal Link(string href, string rel, HttpMethod method)
        {
            this.Href = href;
            this.Rel = rel;
            this.Method = method.Method;
        }

        internal string Href { get; }

        internal string Rel { get; }

        internal string Method { get; }
    }
}
