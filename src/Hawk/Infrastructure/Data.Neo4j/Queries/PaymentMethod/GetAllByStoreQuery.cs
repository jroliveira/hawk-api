namespace Hawk.Infrastructure.Data.Neo4j.Queries.PaymentMethod
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction.Payment;
    using Hawk.Infrastructure.Data.Neo4j.Mappings.Payment;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllByStoreQuery : GetAllQueryBase
    {
        private readonly MethodMapping mapping;

        public GetAllByStoreQuery(
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

        public virtual async Task<Paged<Method>> GetResult(string email, string store, Filter filter)
        {
            var query = this.File.ReadAllText(@"PaymentMethod.GetAllByStore.cql");
            var parameters = new
            {
                email,
                store,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Method>(entities, parameters.skip, parameters.limit);
        }
    }
}