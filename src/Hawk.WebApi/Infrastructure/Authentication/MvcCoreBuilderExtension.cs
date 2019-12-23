namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using static Policies;

    internal static class MvcCoreBuilderExtension
    {
        internal static IMvcCoreBuilder AddAuthorization(this IMvcCoreBuilder @this, IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthConfiguration>();

            if (!authConfig.IsEnabled())
            {
                return @this.AddAuthorization();
            }

            return @this
                .AddAuthorization(options =>
                {
                    options.AddPolicy(AdminPolicy, builder => builder
                        .RequireScope("hawk"));

                    options.AddPolicy(ReadOnlyPolicy, builder => builder
                        .RequireScope("hawk")
                        .RequireScope("hawk.readonly"));
                });
        }
    }
}
