namespace Hawk.Infrastructure.Filter.Data.Neo4J
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
                return 100;
            }

            if (filter.Limit < 1)
            {
                return 100;
            }

            return ToInt32(filter.Limit.Value.Value);
        }
    }
}
