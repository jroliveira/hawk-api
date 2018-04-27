namespace Hawk.Infrastructure.Data.Neo4J.Filter
{
    using System.Collections.Generic;
    using System.Linq;
    using Hawk.Infrastructure.Filter;
    using Http.Query.Filter;
    using Http.Query.Filter.Filters.Condition.Operators;

    internal sealed class Where : IWhere<string, Filter>
    {
        private static readonly IReadOnlyDictionary<Comparison, string> Operations = new Dictionary<Comparison, string>
        {
            { Comparison.GreaterThan, ">=" },
            { Comparison.LessThan, "<=" },
            { Comparison.Equal, "=" },
        };

        public string Apply(Filter filter, string node)
        {
            if (!filter.HasCondition)
            {
                return "1=1";
            }

            var result = new List<string> { "1=1" };

            result.AddRange(filter
                .Where
                .Select(condition => $"{node}.{condition.Field} {GetOperator(condition.Comparison)} {GetValue(condition.Value)}")
                .ToList());

            return string.Join(" AND ", result);
        }

        private static object GetValue(object value)
        {
            switch (value)
            {
                case string s when int.TryParse(s, out var i): return i;
                case string s: return $"\"{s}\"";
                default: return value;
            }
        }

        private static string GetOperator(Comparison comparison) => Operations[comparison];
    }
}
