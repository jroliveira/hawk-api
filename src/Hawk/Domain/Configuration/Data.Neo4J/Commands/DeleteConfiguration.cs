namespace Hawk.Domain.Configuration.Data.Neo4J.Commands
{
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteConfiguration : Command<DeleteParam<string>>, IDeleteConfiguration
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Configuration", "Data.Neo4J", "Commands", "DeleteConfiguration.cql"));
        private readonly Neo4JConnection connection;

        public DeleteConfiguration(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(DeleteParam<string> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                email = param.Email.Value,
                description = param.Id,
            });
    }
}
