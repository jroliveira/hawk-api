namespace Hawk.Infrastructure.Data.Neo4J.Entities.Configuration
{
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Configuration.ConfigurationMapping;

    internal sealed class GetConfigurationByDescription : IGetConfigurationByDescription
    {
        private static readonly Option<string> Statement = ReadCypherScript("Configuration.GetConfigurationByDescription.cql");
        private readonly Neo4JConnection connection;

        public GetConfigurationByDescription(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Configuration>> GetResult(Email email, string description) => this.connection.ExecuteCypherScalar(
            MapConfiguration,
            Statement,
            new
            {
                email = email.ToString(),
                description,
            });
    }
}
