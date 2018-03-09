namespace Hawk.Infrastructure.Data.Neo4J.Queries.PaymentMethod
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities.Payment;
    using Hawk.Domain.Queries.PaymentMethod;
    using Hawk.Infrastructure.Data.Neo4J.Mappings.Payment;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    internal sealed class GetAllQuery : GetAllQueryBase, IGetAllQuery
    {
        private readonly MethodMapping mapping;

        public GetAllQuery(
            Database database,
            MethodMapping mapping,
            GetScript file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, "PaymentMethod.GetAll.cql", limit, skip, where)
        {
            Guard.NotNull(mapping, nameof(mapping), "Method mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Paged<(Method Method, int Count)>> GetResult(string email, Filter filter)
        {
            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.Execute(this.mapping.MapFrom, this.Statement, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Method)
                .ToList();

            return new Paged<(Method, int)>(entities, parameters.skip, parameters.limit);
        }
    }
}