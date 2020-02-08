namespace Hawk.Domain.Configuration.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Configuration.Data.Neo4J.ConfigurationMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class GetConfigurationByDescription : IGetConfigurationByDescription
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Configuration", "Data.Neo4J", "GetConfigurationByDescription.cql"));
        private readonly Neo4JConnection connection;

        public GetConfigurationByDescription(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Configuration>> GetResult(Option<Email> email, Option<string> description) =>
            email
            && description
                ? this.connection.ExecuteCypherScalar(
                    MapConfiguration,
                    Statement,
                    new
                    {
                        email = email.Get().Value,
                        description,
                    })
                : Task(Failure<Configuration>(new NullObjectException("Parameters are required.")));
    }
}
