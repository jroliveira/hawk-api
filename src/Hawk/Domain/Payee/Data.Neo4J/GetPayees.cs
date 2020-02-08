namespace Hawk.Domain.Payee.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.Payee.Data.Neo4J.PayeeMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class GetPayees : IGetPayees
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Payee", "Data.Neo4J", "GetPayees.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetPayees(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        public Task<Try<Page<Try<Payee>>>> GetResult(Option<Email> email, Filter filter) => email.Match(
            async some =>
            {
                var parameters = new
                {
                    email = email.Get().Value,
                    skip = this.skip.Apply(filter),
                    limit = this.limit.Apply(filter),
                };

                var data = await this.connection.ExecuteCypher(MapPayee, Statement, parameters);

                return data.Match<Try<Page<Try<Payee>>>>(
                    _ => _,
                    items => new Page<Try<Payee>>(items, parameters.skip, parameters.limit));
            },
            () => Task(Failure<Page<Try<Payee>>>(new NullObjectException("Email is required."))));
    }
}
