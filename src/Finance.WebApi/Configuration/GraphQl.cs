namespace Finance.WebApi.Configuration
{
    using Finance.WebApi.Lib.Middlewares.GraphQl;

    using GraphQL.Types;

    using Microsoft.AspNetCore.Builder;

    internal static class GraphQl
    {
        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder, Schema schema)
        {
            return builder.UseMiddleware<GraphQlMiddleware>(schema);
        }

        public static IApplicationBuilder UseGraphiQl(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GraphiQlMiddleware>();
        }
    }
}