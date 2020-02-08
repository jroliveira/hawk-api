namespace Hawk.Domain.PaymentMethod.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class GetPaymentMethods : IGetPaymentMethods
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("PaymentMethod", "Data.Neo4J", "GetPaymentMethods.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetPaymentMethods(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        public Task<Try<Page<Try<PaymentMethod>>>> GetResult(Option<Email> email, Filter filter) => email.Match(
            async some =>
            {
                var parameters = new
                {
                    email = some.Value,
                    skip = this.skip.Apply(filter),
                    limit = this.limit.Apply(filter),
                };

                var data = await this.connection.ExecuteCypher(MapPaymentMethod, Statement, parameters);

                return data.Match<Try<Page<Try<PaymentMethod>>>>(
                    _ => _,
                    items => new Page<Try<PaymentMethod>>(items, parameters.skip, parameters.limit));
            },
            () => Task(Failure<Page<Try<PaymentMethod>>>(new NullObjectException("Email is required."))));
    }
}
