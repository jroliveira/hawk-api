namespace Hawk.Domain.Configuration.Data.Neo4J
{
    using System.Linq;
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

    internal sealed class UpsertConfiguration : IUpsertConfiguration
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Configuration", "Data.Neo4J", "UpsertConfiguration.cql"));
        private readonly Neo4JConnection connection;

        public UpsertConfiguration(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Configuration>> Execute(Option<Email> email, Option<string> description, Option<Configuration> entity) =>
            email
            && description
            && entity
                ? this.connection.ExecuteCypherScalar(
                    MapConfiguration,
                    Statement,
                    new
                    {
                        email = email.Get().Value,
                        description,
                        newDescription = entity.Get().Description,
                        type = entity.Get().Type,
                        paymentMethod = entity.Get().PaymentMethod.Value,
                        currency = entity.Get().Currency.Value,
                        payee = entity.Get().Payee.Value,
                        tags = entity.Get().Tags.Select(tag => tag.Value).ToArray(),
                    })
                : Task(Failure<Configuration>(new NullObjectException("Parameters are required.")));
    }
}
