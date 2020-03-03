namespace Hawk.Domain.PaymentMethod.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Linq;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetPaymentMethodsByPayee : Query<GetPaymentMethodsByPayeeParam, Page<Try<PaymentMethod>>>, IGetPaymentMethodsByPayee
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("PaymentMethod", "Data.Neo4J", "Queries", "GetPaymentMethodsByPayee.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetPaymentMethodsByPayee(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        protected override async Task<Try<Page<Try<PaymentMethod>>>> GetResult(GetPaymentMethodsByPayeeParam param)
        {
            var parameters = new
            {
                email = param.Email.Value,
                payee = param.Payee.Id,
                skip = this.skip.Apply(param.Filter),
                limit = this.limit.Apply(param.Filter),
            };

            var data = await this.connection.ExecuteCypher(MapPaymentMethod, Statement, parameters);

            return data.Select(items => new Page<Try<PaymentMethod>>(items, parameters.skip, parameters.limit));
        }
    }
}
