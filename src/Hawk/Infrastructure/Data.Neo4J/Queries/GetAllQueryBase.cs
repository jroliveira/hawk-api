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
            Guard.NotNull(limit, nameof(limit), "Limit cannot be null.");
            Guard.NotNull(skip, nameof(skip), "Skip cannot be null.");
            Guard.NotNull(where, nameof(where), "Where cannot be null.");

            this.Skip = skip;
            this.Limit = limit;
            this.Where = where;
        }

        protected ILimit<int, Filter> Limit { get; }

        protected ISkip<int, Filter> Skip { get; }

        protected IWhere<string, Filter> Where { get; }
    }
}