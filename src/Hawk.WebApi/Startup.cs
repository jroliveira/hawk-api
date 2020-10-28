namespace Hawk.WebApi
{
    using Hawk.Domain;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Resilience;
    using Hawk.WebApi.Features;
    using Hawk.WebApi.Infrastructure.Api;
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.Caching;
    using Hawk.WebApi.Infrastructure.Data.Neo4J;
    using Hawk.WebApi.Infrastructure.ErrorHandling;
    using Hawk.WebApi.Infrastructure.Hal;
    using Hawk.WebApi.Infrastructure.HealthCheck;
    using Hawk.WebApi.Infrastructure.IpRateLimiting;
    using Hawk.WebApi.Infrastructure.Metric;
    using Hawk.WebApi.Infrastructure.Swagger;
    using Hawk.WebApi.Infrastructure.Tracing;
    using Hawk.WebApi.Infrastructure.Versioning;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using static Hawk.Infrastructure.Logging.Logger;
    using static Hawk.Infrastructure.Serialization.JsonSettings;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) => services
            .ConfigureCache()
            .ConfigureIpRateLimiting(this.Configuration)
            .ConfigureResilience(this.Configuration)
            .ConfigureNeo4J(this.Configuration)
            .ConfigureFilter()
            .ConfigureDomain()
            .ConfigureFeature()
            .ConfigureApi(
                mvcCoreBuilder => mvcCoreBuilder
                    .AddAuthorization(this.Configuration)
                    .AddHal(JsonSerializerSettings),
                mvcOptions => mvcOptions
                    .AddApiVersionRoutePrefixConvention()
                    .AddAuthorizeFilter(services, this.Configuration))
            .ConfigureHealthCheck(healthChecksBuilder => healthChecksBuilder
                .AddNeo4JCheck()
                .AddAuthenticationCheck(this.Configuration))
            .ConfigureVersioning()
            .ConfigureMetric()
            .ConfigureSwagger(this.Configuration)
            .ConfigureTracing(this.Configuration)
            .ConfigureAuthentication(this.Configuration);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IHostApplicationLifetime lifetime)
        {
            app
                .UseAuthentication()
                .UseErrorHandling(environment)
                .UseApi()
                .UseHealthCheck()
                .UseMetric()
                .UseSwagger(this.Configuration)
                .UseTracing(this.Configuration);

            lifetime.ApplicationStarted.Register(() => LogInfo("Web Api started", new
            {
                Environment = environment.EnvironmentName,
                Port = this.Configuration["app:port"],
            }));

            lifetime.ApplicationStopped.Register(() => LogError("Web Api stopped"));
        }
    }
}
