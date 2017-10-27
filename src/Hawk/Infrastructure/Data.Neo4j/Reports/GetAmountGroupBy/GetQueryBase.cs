namespace Hawk.Infrastructure.Data.Neo4j.Reports.GetAmountGroupBy
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Data.Neo4j.Queries;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    public abstract class GetQueryBase : GetAllQueryBase
    {
        private readonly ItemMapping mapping;

        protected GetQueryBase(
            Database database,
            ItemMapping mapping,
            GetScript file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, limit, skip, where)
        {
            this.mapping = mapping;
        }

        public virtual async Task<Paged<Item>> GetResultAsync(string email, Filter filter)
        {
            var query = this.GetQueryString();
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

            return new Paged<Item>(entities, parameters.skip, parameters.limit);
        }

        protected abstract string GetQueryString();
    }
}