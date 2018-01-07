namespace Hawk.Infrastructure.Data.Neo4J.Queries.Store
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Store;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    internal sealed class GetAllQuery : GetAllQueryBase, IGetAllQuery
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

        public async Task<Paged<Store>> GetResult(string email, Filter filter)
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