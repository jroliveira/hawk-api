namespace Hawk.WebApi.Infrastructure.Tracing
{
    using Hawk.Infrastructure.Tracing.Configurations;

    using Jaeger;
    using Jaeger.Reporters;
    using Jaeger.Samplers;
    using Jaeger.Senders;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using OpenTracing;

    using static OpenTracing.Util.GlobalTracer;

    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection ConfigureTracing(this IServiceCollection @this, IConfiguration configuration) => @this
            .AddOpenTracing()
            .AddSingleton<ITracer>(serviceProvider =>
            {
                var serviceName = serviceProvider
                    .GetRequiredService<IWebHostEnvironment>()
                    .ApplicationName;

                var tracingConfig = configuration
                    .GetSection("tracing")
                    .Get<TracingConfiguration>();

                if (!tracingConfig.IsEnabled())
                {
                    return new Tracer
                        .Builder(serviceName)
                        .WithReporter(new NoopReporter())
                        .WithSampler(new ConstSampler(false))
                        .Build();
                }

                var (_, (agentHost, agentPort)) = tracingConfig;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                var tracer = new Tracer
                    .Builder(serviceName)
                    .WithSampler(new ConstSampler(true))
                    .WithReporter(new RemoteReporter
                        .Builder()
                        .WithSender(new UdpSender(agentHost, agentPort, 0))
                        .WithLoggerFactory(loggerFactory)
                        .Build())
                    .Build();

                Register(tracer);

                return tracer;
            });
    }
}
