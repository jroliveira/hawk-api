namespace Hawk.Infrastructure.Data.Neo4J.Entities.Configuration
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Configuration.ConfigurationMapping;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertConfiguration : IUpsertConfiguration
    {
        private static readonly Option<string> Statement = ReadCypherScript("Configuration\\UpsertConfiguration.cql");
        private readonly Neo4JConnection connection;

        public UpsertConfiguration(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Configuration>> Execute(Email email, Option<Configuration> entity)
        {
            if (!entity.IsDefined)
            {
                return Task(Failure<Configuration>(new NullReferenceException("Configuration is required.")));
            }

            return this.connection.ExecuteCypherScalar(
                MapConfiguration,
                Statement,
                new
                {
                    email = email.ToString(),
                    description = entity.Get().Description,
                    type = entity.Get().Type,
                    paymentMethod = entity.Get().PaymentMethod.Name,
                    currency = entity.Get().Currency.Name,
                    store = entity.Get().Store.Name,
                    tags = entity.Get().Tags.Select(tag => tag.Name).ToArray(),
                });
        }
    }
}
