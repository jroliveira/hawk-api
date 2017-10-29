namespace Hawk.WebApi.Configuration
{
    using GraphQL.Types;

    using Hawk.Infrastructure.GraphQl;
    using Hawk.WebApi.Lib.Middlewares.GraphQl;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    internal static class GraphQl
    {
        internal static IServiceCollection ConfigureGraphQl(
            this IServiceCollection services, 
            IConfigurationRoot configuration)
        {
            services.Configure<GraphQlConfig>(configuration.GetSection("graphql"));

            return services;
        }

        internal static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder, 
            Schema schema, 
            IOptions<GraphQlConfig> graphQlConfig)
        {
            return builder.UseMiddleware<GraphQlMiddleware>(schema, graphQlConfig);
        }

        internal static IApplicationBuilder UseGraphiQl(
            this IApplicationBuilder app, 
            IOptions<GraphQlConfig> graphQlConfig)
        {
            return app.UseMiddleware<GraphiQlMiddleware>(graphQlConfig);
        }
    }
}