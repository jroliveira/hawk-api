namespace Finance.Infrastructure.Data.Neo4j.Queries.PaymentMethod
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction.Payment;
    using Finance.Infrastructure.Data.Neo4j.Mappings.Payment;
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery : GetAllQueryBase
    {
        private readonly MethodMapping mapping;

        public GetAllQuery(
            Database database,
            MethodMapping mapping,
            GetScript file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, limit, skip, where)
        {
            this.mapping = mapping;
        }

        public virtual async Task<Paged<Method>> GetResultAsync(string email, Filter filter)
        {
            var query = this.File.ReadAllText(@"PaymentMethod.GetAll.cql");
            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.ExecuteAsync(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Method>(entities, parameters.skip, parameters.limit);
        }
    }
}