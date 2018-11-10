namespace Hawk.WebApi
{
    using Hawk.Infrastructure.IoC;
    using Hawk.WebApi.Configuration;
    using Hawk.WebApi.Lib;
    using Hawk.WebApi.Lib.Middlewares;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureIpRateLimiting(this.Configuration)
                .ConfigureIoC(this.Configuration)
                .ConfigureApi()
                .ConfigureSwagger();

            this.ConfigureAuth(services);
        }

        public void Configure(IApplicationBuilder app, IHttpContextAccessor accessor) => app
            .UseLog(this.Configuration, accessor)
            .UseResponseCaching()
            .UseResponseCompression()
            .UseMiddleware<ErrorHandlingMiddleware>()
            .UseMiddleware<SecurityHeadersMiddleware>()
            .UseMiddleware<HttpLogMiddleware>()
            .UseCors(Constants.Api.Cors)
            .UseAuthentication()
            .UseMvc()
            .UseSwagger();

        protected virtual void ConfigureAuth(IServiceCollection services) => services.ConfigureIdentityServer(this.Configuration);
    }
}
