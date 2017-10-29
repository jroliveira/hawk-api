namespace Hawk.WebApi
{
    using AutoMapper;

    using Hawk.Infrastructure.GraphQl;
    using Hawk.WebApi.Configuration;
    using Hawk.WebApi.GraphQl.Schemas;
    using Hawk.WebApi.Lib.Middlewares;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAutoMapper()
                .ConfigureDatabase(this.Configuration)
                .ConfigureGraphQl(this.Configuration)
                .ConfigureIoC(this.Configuration)
                .ConfigureApi(this.Configuration)
                .ConfigureIdentityServer();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            HawkSchema schema,
            IOptions<GraphQlConfig> graphQlConfig)
        {
            app
                .UseMiddleware<HandlerErrorMiddleware>()
                .UseGraphQl(schema, graphQlConfig)
                .UseGraphiQl(graphQlConfig)
                .UseCors("CorsPolicy")
                .UseAuthentication()
                .UseMvc();
        }
    }
}
