namespace Hawk.Domain.Currency.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.Currency.Data.Neo4J.CurrencyMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetCurrencies : Query<GetAllParam, Page<Try<Currency>>>, IGetCurrencies
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Currency", "Data.Neo4J", "Queries", "GetCurrencies.cql"));
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

        protected override async Task<Try<Page<Try<Currency>>>> GetResult(GetAllParam param)
        {
            var parameters = new
            {
                email = param.Email.Value,
                skip = this.skip.Apply(param.Filter),
                limit = this.limit.Apply(param.Filter),
            };

            var data = await this.connection.ExecuteCypher(MapCurrency, Statement, parameters);

            return data.Match<Try<Page<Try<Currency>>>>(
                _ => _,
                items => new Page<Try<Currency>>(items, parameters.skip, parameters.limit));
        }
    }
}
