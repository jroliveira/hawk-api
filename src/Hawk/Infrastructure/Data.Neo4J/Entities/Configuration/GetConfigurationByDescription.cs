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
        private static readonly Option<string> Statement = ReadAll("Configuration.GetConfigurationByDescription.cql");
        private readonly Database database;

        public GetConfigurationByDescription(Database database) => this.database = database;

        public Task<Try<Configuration>> GetResult(Email email, string description)
        {
            var parameters = new
            {
                email = email.ToString(),
                description,
            };

            return this.database.ExecuteScalar(MapFrom, Statement, parameters);
        }
    }
}
