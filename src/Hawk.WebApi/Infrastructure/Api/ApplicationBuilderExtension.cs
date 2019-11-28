namespace Hawk.WebApi.Infrastructure.Api
{
    using Microsoft.AspNetCore.Builder;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseApi(this IApplicationBuilder @this) => @this
            .UseMiddleware<SecurityHeadersMiddleware>()
            .UseResponseCaching()
            .UseResponseCompression()
            .UseCors("CorsPolicy")
            .UseRouting()
            .UseHttpsRedirection()
            .UseMvc();
    }
}
