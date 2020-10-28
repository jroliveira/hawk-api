namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.WebApi.Infrastructure.Authentication.Configurations;
    using Hawk.WebApi.Infrastructure.HealthCheck;

    using Microsoft.Extensions.Options;

    internal sealed class AuthorityDnsHealthCheck : DnsHealthCheck
    {
        public AuthorityDnsHealthCheck(IOptions<AuthConfiguration> config)
            : base(config.Value.Authority?.Host)
        {
        }
    }
}
