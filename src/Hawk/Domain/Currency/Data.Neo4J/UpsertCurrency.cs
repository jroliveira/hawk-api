namespace Hawk.Domain.Currency.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;
    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertCurrency : IUpsertCurrency
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Currency", "Data.Neo4J", "UpsertCurrency.cql"));
        private readonly Neo4JConnection connection;

        public UpsertCurrency(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Currency>> Execute(Email email, string name, Option<Currency> entity) => entity.Match(
            async some =>
            {
                var data = await this.connection.ExecuteCypherScalar(
                    MapCurrency,
                    Statement,
                    new
                    {
                        email = email.Value,
                        name,
                        newName = some.Value,
                    });

                return data.Match<Try<Currency>>(
                    _ => _,
                    currency => currency.Currency);
            },
            () => Task(Failure<Currency>(new NullObjectException("Currency is required."))));
    }
}
