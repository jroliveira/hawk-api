namespace Hawk.Domain.Currency.Data.Neo4J.Commands
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Currency.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.IO.Path;
    using static System.String;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class UpsertCurrency : Command<UpsertParam<string, Currency>>, IUpsertCurrency
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Currency", "Data.Neo4J", "Commands", "UpsertCurrency.cql"));
        private readonly Neo4JConnection connection;

        public UpsertCurrency(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(UpsertParam<string, Currency> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                email = param.Email.Value,
                code = param.Id,
                newCode = param.Entity.Id,
                newSymbol = param.Entity.SymbolOption.GetOrElse(Empty),
            });
    }
}
