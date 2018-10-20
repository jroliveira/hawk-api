namespace Hawk.Infrastructure.Data.Neo4J.Queries.PaymentMethod
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities.Payment;
    using Hawk.Domain.Queries.PaymentMethod;
    using Hawk.Infrastructure.Data.Neo4J.Mappings.Payment;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Http.Query.Filter;

    using static System.String;

    internal sealed class GetAllQuery : IGetAllQuery
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("PaymentMethod.GetAll.cql");
        private readonly Database database;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetAllQuery(
            Database database,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.database = database;
            this.limit = limit;
            this.skip = skip;
        }

        public async Task<Try<Paged<Try<(Method Method, uint Count)>>>> GetResult(string email, Filter filter)
        {
            var parameters = new
            {
                email,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.database.Execute(MethodMapping.MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.Match<Try<Paged<Try<(Method, uint)>>>>(
                _ => _,
                items => new Paged<Try<(Method, uint)>>(items.ToList(), parameters.skip, parameters.limit));
        }
    }
}