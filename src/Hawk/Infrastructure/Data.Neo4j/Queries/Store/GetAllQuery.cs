namespace Hawk.Infrastructure.Data.Neo4j.Queries.Store
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction.Details;
    using Hawk.Infrastructure.Data.Neo4j.Mappings;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery : GetAllQueryBase
    {
        private readonly StoreMapping mapping;

        public GetAllQuery(
            Database database,
            StoreMapping mapping,
            GetScript file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, limit, skip, where)
        {
            this.mapping = mapping;
        }

        public virtual async Task<Paged<Store>> GetResult(string email, Filter filter)
        {
            var query = this.File.ReadAllText(@"Store.GetAll.cql");
            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Store>(entities, parameters.skip, parameters.limit);
        }
    }
}