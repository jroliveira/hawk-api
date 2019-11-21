namespace Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod.PaymentMethodMapping;

    internal sealed class GetPaymentMethodsByStore : IGetPaymentMethodsByStore
    {
        private static readonly Option<string> Statement = ReadCypherScript("PaymentMethod.GetPaymentMethodsByStore.cql");
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetPaymentMethodsByStore(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        public async Task<Try<Page<Try<(PaymentMethod PaymentMethod, uint Count)>>>> GetResult(Email email, string store, Filter filter)
        {
            var parameters = new
            {
                email = email.ToString(),
                store,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.connection.ExecuteCypher(MapPaymentMethod, Statement, parameters);

            return data.Match<Try<Page<Try<(PaymentMethod, uint)>>>>(
                _ => _,
                items => new Page<Try<(PaymentMethod, uint)>>(items, parameters.skip, parameters.limit));
        }
    }
}
