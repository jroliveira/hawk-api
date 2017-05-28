namespace Finance.Infrastructure.Data.Neo4j.Filter
{
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class Limit : ILimit<int, Filter>
    {
        public int Apply(Filter filter)
        {
            if (filter.Limit == null)
            {
                return 10000;
            }

            if (filter.Limit < 1)
            {
                return 10000;
            }

            return filter.Limit;
        }
    }
}
