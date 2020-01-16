namespace Hawk.Domain.Currency.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteCurrency : IDeleteCurrency
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Currency", "Data.Neo4J", "DeleteCurrency.cql"));
        private readonly Neo4JConnection connection;

        public DeleteCurrency(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Unit>> Execute(Email email, string name) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = email.Value,
                name,
            });
    }
}
