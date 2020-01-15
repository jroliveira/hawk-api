namespace Hawk.Domain.Configuration.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Configuration.Data.Neo4J.ConfigurationMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetConfigurationByDescription : IGetConfigurationByDescription
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Configuration", "Data.Neo4J", "GetConfigurationByDescription.cql"));
        private readonly Neo4JConnection connection;

        public GetConfigurationByDescription(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Configuration>> GetResult(Email email, string description) => this.connection.ExecuteCypherScalar(
            MapConfiguration,
            Statement,
            new
            {
                email = email.Value,
                description,
            });
    }
}
