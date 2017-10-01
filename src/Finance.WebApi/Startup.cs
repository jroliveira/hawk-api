namespace Finance.WebApi
{
    using AutoMapper;

    using Finance.Infrastructure.Data.Neo4j.Queries.Transaction;
    using Finance.WebApi.Configuration;
    using Finance.WebApi.GraphQl.Schemas;
    using Finance.WebApi.Lib.Middlewares;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

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
                .ConfigureIoC(this.Configuration)
                .ConfigureApi(this.Configuration);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            TransactionSchema schema)
        {
            loggerFactory
                .AddConsole(this.Configuration.GetSection("Logging"))
                .AddDebug();

            app
                .UseMiddleware<HandlerErrorMiddleware>()
                .UseGraphQl(schema)
                .UseGraphiQl()
                .UseIdentityServer()
                .UseCors("CorsPolicy")
                .UseMvc();
        }
    }
}
