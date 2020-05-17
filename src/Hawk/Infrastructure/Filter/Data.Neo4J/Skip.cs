namespace Hawk.Infrastructure.Filter.Data.Neo4J
{
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    using static System.Convert;

    internal sealed class Skip : ISkip<int, Filter>
    {
        public int Apply(Filter filter) => ToInt32(filter.Skip.GetOrElse(0));
    }
}
