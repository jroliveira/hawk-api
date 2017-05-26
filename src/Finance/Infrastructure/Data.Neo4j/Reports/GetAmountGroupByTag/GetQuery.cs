namespace Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupByTag
{
    using System.Linq;

    using Finance.Infrastructure.Data.Neo4j.Queries;
    using Finance.Infrastructure.Filter;
    using Finance.Reports.GetAmountGroupByTag;

    using Http.Query.Filter;

    public class GetQuery : GetAllQueryBase
    {
        private readonly TagMapping mapping;

        public GetQuery(
            Database database,
            TagMapping mapping,
            GetScript file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, limit, skip, where)
        {
            this.mapping = mapping;
        }

        public virtual Paged<Tag> GetResult(string email, Filter filter)
        {
            var query = this.File.ReadAllText(@"GetAmountGroupByTag\Query.cql");
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

            return new Paged<Tag>(entities, parameters.skip, parameters.limit);
        }
    }
}