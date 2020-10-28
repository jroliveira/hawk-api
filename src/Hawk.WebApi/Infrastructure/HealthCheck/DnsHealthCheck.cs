namespace Hawk.WebApi.Infrastructure.HealthCheck
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;

    using static System.Net.Dns;

    using static Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult;

    internal abstract class DnsHealthCheck : IHealthCheck
    {
        private readonly string? host;

        protected DnsHealthCheck(string? host) => this.host = host;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var entry = await GetHostEntryAsync(this.host);

            return entry.AddressList.Any()
                ? Healthy()
                : Unhealthy();
        }
    }
}
