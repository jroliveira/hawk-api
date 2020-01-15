namespace Hawk.Infrastructure.Filter.Data.Neo4J
{
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    using static System.Convert;

    internal sealed class Skip : ISkip<int, Filter>
    {
        public int Apply(Filter filter)
        {
            if (filter.Skip.Value == null)
            {
                return 0;
            }

            if (filter.Skip < 1)
            {
                return 0;
            }

            return ToInt32(filter.Skip.Value.Value);
        }
    }
}
