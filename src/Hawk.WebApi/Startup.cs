namespace Hawk.WebApi
{
    using AspNetCoreRateLimit;
    using Hawk.Infrastructure.IoC;
    using Hawk.WebApi.Configuration;
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Middlewares;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal sealed class Startup
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
                .ConfigureIpRateLimiting(this.Configuration)
                .ConfigureDatabase(this.Configuration)
                .ConfigureLog(this.Configuration)
                .ConfigureIoC()
                .ConfigureApi()
                .ConfigureSwagger()
                .ConfigureIdentityServer(this.Configuration);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            app
                .UseResponseCaching()
                .UseResponseCompression()
                .UseIpRateLimiting()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseMiddleware<SecurityHeadersMiddleware>()
                .UseMiddleware<HttpLogMiddleware>()
                .UseCors(Constants.Api.Cors)
                .UseAuthentication()
                .UseMvc()
                .UseSwagger();
        }
    }
}
