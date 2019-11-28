namespace Hawk.WebApi.Infrastructure.Hal
{
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    using static System.String;
    using static System.Web.HttpUtility;

    internal static class HttpContextExtension
    {
        internal static string GetQueryString(this HttpContext @this, params string[] ignored)
        {
            var queryString = ParseQueryString(Empty);

            foreach (var (key, value) in @this.Request.Query.Where(item => !ignored.Contains(item.Key)))
            {
                queryString.Add(key, value.FirstOrDefault());
            }

            return queryString.Count > 0
                ? $"&{queryString}"
                : Empty;
        }

        internal static string GetUri(this HttpContext @this) => @this.Request.Path;
    }
}
