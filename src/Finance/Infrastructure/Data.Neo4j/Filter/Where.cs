namespace Finance.Infrastructure.Data.Neo4j.Filter
{
    using System.Collections.Generic;
    using System.Linq;

    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;
    using Http.Query.Filter.Filters.Condition.Operators;

    using static System.Int32;

    public class Where : IWhere<string, Filter>
    {
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
                case string s when TryParse(s, out var i): return i;
                case string s: return $"\"{s}\"";
                default: return value;
            }
        }

        private static string GetOperator(Comparison comparison)
        {
            var operations = new Dictionary<Comparison, string>
            {
                { Comparison.GreaterThan, ">=" },
                { Comparison.LessThan, "<=" },
                { Comparison.Equal, "=" }
            };

            return operations[comparison];
        }
    }
}
