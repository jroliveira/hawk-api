namespace Finance.Infrastructure.Data.Neo4j.Queries
{
    using Finance.Infrastructure.Data.Neo4j.Mappings;
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQueryBase<TReturn> : QueryBase<TReturn>
    {
        public GetAllQueryBase(
            Database database,
            IMapping<TReturn> mapping,
            File file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, mapping, file)
        {
            this.Skip = skip;
            this.Limit = limit;
            this.Where = where;
        }

        protected ILimit<int, Filter> Limit { get; }

        protected ISkip<int, Filter> Skip { get; }

        protected IWhere<string, Filter> Where { get; }
    }
}