namespace Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupBy
{
    using System.Linq;

    using Finance.Infrastructure.Data.Neo4j.Queries;
    using Finance.Infrastructure.Filter;
    using Finance.Reports.GetAmountGroupBy;

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

        public virtual Paged<Item> GetResult(string email, Filter filter)
        {
            var query = this.GetQueryString();
            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = this.Database.Execute(this.mapping.MapFrom, query, parameters);
            var entities = data
                .OrderBy(item => item.Name)
                .ToList();

            return new Paged<Item>(entities, parameters.skip, parameters.limit);
        }

        protected abstract string GetQueryString();
    }
}