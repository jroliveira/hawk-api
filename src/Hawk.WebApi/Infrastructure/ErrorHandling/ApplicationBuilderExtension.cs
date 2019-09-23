namespace Hawk.WebApi.Infrastructure.ErrorHandling
{
    using Microsoft.AspNetCore.Builder;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseErrorHandling(this IApplicationBuilder @this) => @this
            .UseMiddleware<ErrorHandlingMiddleware>();
    }
}
