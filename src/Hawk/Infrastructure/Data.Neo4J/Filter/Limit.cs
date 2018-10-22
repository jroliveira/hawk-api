namespace Hawk.Infrastructure.Data.Neo4J.Filter
{
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    using static System.Convert;

    internal sealed class Limit : ILimit<int, Filter>
    {
        public int Apply(Filter filter)
        {
            if (filter.Limit.Value == null)
            {
                return 10000;
            }

            if (filter.Limit < 1)
            {
                return 10000;
            }

            return ToInt32(filter.Limit.Value.Value);
        }
    }
}
