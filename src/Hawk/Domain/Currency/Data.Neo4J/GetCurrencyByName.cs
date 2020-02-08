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

    internal sealed class GetCurrencyByName : IGetCurrencyByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Currency", "Data.Neo4J", "GetCurrencyByName.cql"));
        private readonly Neo4JConnection connection;

        public GetCurrencyByName(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Currency>> GetResult(Option<Email> email, Option<string> name) =>
            email
            && name
                ? this.connection.ExecuteCypherScalar(
                    MapCurrency,
                    Statement,
                    new
                    {
                        email = email.Get().Value,
                        name = name.Get(),
                    })
                : Task(Failure<Currency>(new NullObjectException("Parameters are required.")));
    }
}
