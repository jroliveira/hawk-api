namespace Hawk.Domain.Currency.Data.Neo4J.Commands
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteCurrency : Command<DeleteParam<string>>, IDeleteCurrency
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Currency", "Data.Neo4J", "Commands", "DeleteCurrency.cql"));
        private readonly Neo4JConnection connection;

        public DeleteCurrency(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(DeleteParam<string> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                email = param.Email.Value,
                code = param.Id,
            });
    }
}
