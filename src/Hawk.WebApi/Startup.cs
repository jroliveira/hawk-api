namespace Hawk.WebApi
{
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.WebApi.Infrastructure.Api;
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.ErrorHandling;
    using Hawk.WebApi.Infrastructure.IpRateLimiting;
    using Hawk.WebApi.Infrastructure.Logging;
    using Hawk.WebApi.Infrastructure.Metric;
    using Hawk.WebApi.Infrastructure.Swagger;

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
                .ConfigureNeo4J(this.Configuration)
                .ConfigureApi(this.Configuration)
                .ConfigureSwagger()
                .ConfigureMetric();

            this.ConfigureAuthentication(services);
        }

        public void Configure(IApplicationBuilder app, IHttpContextAccessor accessor) => app
            .UseLogging(this.Configuration, accessor)
            .UseAuthentication()
            .UseErrorHandling()
            .UseApi()
            .UseSwagger()
            .UseMetric();

        protected virtual void ConfigureAuthentication(IServiceCollection services) => services.ConfigureAuthentication(this.Configuration);
    }
}
