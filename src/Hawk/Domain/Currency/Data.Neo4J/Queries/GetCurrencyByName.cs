namespace Hawk.Domain.Currency.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetCurrencyByName : Query<GetByIdParam<string>, Currency>, IGetCurrencyByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Currency", "Data.Neo4J", "Queries", "GetCurrencyByName.cql"));
        private readonly Neo4JConnection connection;

        public GetCurrencyByName(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Currency>> GetResult(GetByIdParam<string> param) => this.connection.ExecuteCypherScalar(
            MapCurrency,
            Statement,
            new
            {
                email = param.Email.Value,
                name = param.Id,
            });
    }
}
