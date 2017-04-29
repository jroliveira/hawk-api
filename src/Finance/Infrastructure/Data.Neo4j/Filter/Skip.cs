namespace Finance.Infrastructure.Data.Neo4j.Filter
{
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class Skip : ISkip<int, Filter>
    {
        public int Apply(Filter filter)
        {
            if (filter.Skip == null)
            {
                return 0;
            }

            if (filter.Skip < 1)
            {
                return 0;
            }

            return filter.Skip;
        }
    }
}