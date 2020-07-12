namespace Hawk.Domain.Configuration.Data.Neo4J.Commands
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Configuration.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class UpsertConfiguration : Command<UpsertParam<string, Configuration>>, IUpsertConfiguration
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Configuration", "Data.Neo4J", "Commands", "UpsertConfiguration.cql"));
        private readonly Neo4JConnection connection;

        public UpsertConfiguration(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(UpsertParam<string, Configuration> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                email = param.Email.Value,
                description = param.Id,
                newDescription = param.Entity.Id,
                type = param.Entity.Type,
                paymentMethod = param.Entity.PaymentMethod.Id,
                currency = param.Entity.Currency.Id,
                payee = param.Entity.Payee.Id,
                category = param.Entity.Category.Id,
                tags = param.Entity.Tags.Select(tag => tag.Id).ToArray(),
            });
    }
}
