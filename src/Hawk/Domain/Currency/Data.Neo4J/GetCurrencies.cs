namespace Hawk.Domain.Currency.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;
    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetCurrencies : IGetCurrencies
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Currency", "Data.Neo4J", "GetCurrencies.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetCurrencies(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        public async Task<Try<Page<Try<(Currency Currency, uint Count)>>>> GetResult(Email email, Filter filter)
        {
            var parameters = new
            {
                email = email.Value,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.connection.ExecuteCypher(MapCurrency, Statement, parameters);

            return data.Match<Try<Page<Try<(Currency, uint)>>>>(
                _ => _,
                items => new Page<Try<(Currency, uint)>>(items, parameters.skip, parameters.limit));
        }
    }
}
