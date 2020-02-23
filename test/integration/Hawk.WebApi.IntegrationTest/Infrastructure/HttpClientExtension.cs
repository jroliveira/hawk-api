namespace Hawk.WebApi.IntegrationTest.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using static System.Text.Encoding;

    using static Hawk.Infrastructure.Extensions.DynamicExtension;

    using static Newtonsoft.Json.JsonConvert;

    internal static class HttpClientExtension
    {
        private static readonly IReadOnlyDictionary<string, HttpMethod> Methods = new Dictionary<string, HttpMethod>
        {
            { "DELETE", HttpMethod.Delete },
            { "GET", HttpMethod.Get },
            { "POST", HttpMethod.Post },
            { "PUT", HttpMethod.Put },
        };

        internal static Task<(HttpStatusCode StatusCode, dynamic? Response)> Get(this HttpClient @this, string url) => @this.Request(new
        {
            Url = url,
            Method = "GET",
            Headers = new Dictionary<string, dynamic>
            {
                { "X-Email", "junior@gmail.com" },
            },
        });

        internal static Task<(HttpStatusCode StatusCode, dynamic? Response)> Delete(this HttpClient @this, string url) => @this.Request(new
        {
            Url = url,
            Method = "DELETE",
            Headers = new Dictionary<string, dynamic>
            {
                { "X-Email", "junior@gmail.com" },
            },
        });

        internal static Task<(HttpStatusCode StatusCode, dynamic? Response)> Post(this HttpClient @this, string url, object data) => @this.Request(new
        {
            Url = url,
            Method = "POST",
            Headers = new Dictionary<string, dynamic>
            {
                { "Content-Type", "application/json" },
                { "X-Email", "junior@gmail.com" },
            },
            Data = data,
        });

        internal static async Task<(HttpStatusCode StatusCode, dynamic? Response)> Request(this HttpClient @this, dynamic options)
        {
            using var request = new HttpRequestMessage(Methods[options.Method], options.Url);
            ConfigureHeaders(request, options.Headers);

            if (IsPropertyExist(options, "Data"))
            {
                request.Content = new StringContent(SerializeObject(options.Data), UTF8, "application/json");
            }

            using var response = await @this.SendAsync(request);

            return (response.StatusCode, DeserializeObject(await response.Content.ReadAsStringAsync()));
        }

        private static void ConfigureHeaders(this HttpRequestMessage @this, IDictionary<string, dynamic> headers)
        {
            foreach (var (key, value) in headers.Where(header => !header.Key.Equals("Content-Type")))
            {
                @this.Headers.Add(key, value);
            }
        }
    }
}
