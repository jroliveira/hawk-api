namespace Hawk.WebApi.Infrastructure.Api
{
    using Microsoft.AspNetCore.Builder;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseApi(this IApplicationBuilder @this) => @this
            .UseMiddleware<ErrorHandlingMiddleware>()
            .UseMiddleware<SecurityHeadersMiddleware>()
            .UseResponseCaching()
            .UseResponseCompression()
            .UseCors("CorsPolicy")
            .UseMvc();
    }
}
