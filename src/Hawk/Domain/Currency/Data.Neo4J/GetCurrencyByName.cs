namespace Hawk.Domain.Currency.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;
    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetCurrencyByName : IGetCurrencyByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Currency", "Data.Neo4J", "GetCurrencyByName.cql"));
        private readonly Neo4JConnection connection;

        public GetCurrencyByName(Neo4JConnection connection) => this.connection = connection;

        public async Task<Try<Currency>> GetResult(Email email, string name)
        {
            var data = await this.connection.ExecuteCypherScalar(
                MapCurrency,
                Statement,
                new
                {
                    email = email.Value,
                    name,
                });

            return data.Match<Try<Currency>>(
                _ => _,
                currency => currency.Currency);
        }
    }
}
