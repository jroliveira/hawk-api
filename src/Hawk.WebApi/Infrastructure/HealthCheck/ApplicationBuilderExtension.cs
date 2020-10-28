namespace Hawk.WebApi.Infrastructure.HealthCheck
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Http;

    using static System.Net.Mime.MediaTypeNames.Application;

    using static Hawk.Infrastructure.Serialization.JsonSettings;

    using static Microsoft.AspNetCore.Http.StatusCodes;
    using static Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;

    using static Newtonsoft.Json.JsonConvert;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseHealthCheck(this IApplicationBuilder @this) => @this.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health/live", GetHealthCheckOptions("live"));
            endpoints.MapHealthChecks("/health/ready", GetHealthCheckOptions("ready"));
        });

        internal static HealthCheckOptions GetHealthCheckOptions(string tag) => new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains(tag),
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = Json;
                await context.Response.WriteAsync(SerializeObject(report, JsonSerializerSettings));
            },
            ResultStatusCodes =
            {
                [Healthy] = Status200OK,
                [Degraded] = Status200OK,
                [Unhealthy] = Status503ServiceUnavailable,
            },
        };
    }
}
