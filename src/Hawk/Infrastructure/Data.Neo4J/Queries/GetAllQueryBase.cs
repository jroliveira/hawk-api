namespace Hawk.Infrastructure.Data.Neo4J.Queries
{
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    internal class GetAllQueryBase : QueryBase
    {
        public GetAllQueryBase(
            Database database,
            File file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file)
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