namespace Hawk.WebApi.Infrastructure.Data.Neo4J
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Data.Neo4J;

    using Microsoft.Extensions.Diagnostics.HealthChecks;

    using static Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult;

    internal sealed class Neo4JHealthCheck : IHealthCheck
    {
        private readonly ICheckNeo4J checkNeo4J;

        public Neo4JHealthCheck(ICheckNeo4J checkNeo4J) => this.checkNeo4J = checkNeo4J;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default) => await this.checkNeo4J.Execute()
                ? Healthy()
                : Unhealthy();
    }
}
