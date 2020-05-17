namespace Hawk.Infrastructure.Filter.Data.Neo4J
{
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    using static System.Convert;

    internal sealed class Limit : ILimit<int, Filter>
    {
        public int Apply(Filter filter) => ToInt32(filter.Limit.GetOrElse(100));
    }
}
