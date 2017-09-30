namespace Finance.WebApi.Configuration
{
    using Finance.WebApi.GraphQl.Schemas;
    using Finance.WebApi.Lib.Middlewares.GraphQl;

    using Microsoft.AspNetCore.Builder;

    internal static class GraphQl
    {
        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder)
        {
            var schema = new StarWarsSchema();

            return builder.UseMiddleware<GraphQlMiddleware>(schema);
        }

        public static IApplicationBuilder UseGraphiQl(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GraphiQlMiddleware>();
        }
    }
}