namespace Hawk.Infrastructure.Data.Neo4J.Filter
{
    using Hawk.Infrastructure.Filter;
    using Http.Query.Filter;

    internal sealed class Skip : ISkip<uint, Filter>
    {
        public uint Apply(Filter filter)
        {
            if (filter.Skip.Value == null)
            {
                return 0;
            }

            if (filter.Skip < 1)
            {
                return 0;
            }

            return filter.Skip.Value.Value;
        }
    }
}
