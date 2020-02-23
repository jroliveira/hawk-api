namespace Hawk.Domain.Configuration.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Configuration.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Configuration.Data.Neo4J.ConfigurationMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetConfigurationByDescription : Query<GetByIdParam<string>, Configuration>, IGetConfigurationByDescription
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Configuration", "Data.Neo4J", "Queries", "GetConfigurationByDescription.cql"));
        private readonly Neo4JConnection connection;

        public GetConfigurationByDescription(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Configuration>> GetResult(GetByIdParam<string> param) => this.connection.ExecuteCypherScalar(
            MapConfiguration,
            Statement,
            new
            {
                email = param.Email.Value,
                description = param.Id,
            });
    }
}
