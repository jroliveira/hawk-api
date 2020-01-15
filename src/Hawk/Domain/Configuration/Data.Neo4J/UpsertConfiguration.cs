namespace Hawk.Domain.Configuration.Data.Neo4J
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
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

        public Task<Try<Configuration>> Execute(Email email, Option<Configuration> entity) => entity.Match(
            some => this.connection.ExecuteCypherScalar(
                MapConfiguration,
                Statement,
                new
                {
                    email = email.Value,
                    description = some.Description,
                    type = some.Type,
                    paymentMethod = some.PaymentMethod.Value,
                    currency = some.Currency.Value,
                    store = some.Store.Value,
                    tags = some.Tags.Select(tag => tag.Value).ToArray(),
                }),
            () => Task(Failure<Configuration>(new NullReferenceException("Configuration is required."))));
    }
}
