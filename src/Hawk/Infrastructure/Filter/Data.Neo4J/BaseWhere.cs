namespace Hawk.Infrastructure.Filter.Data.Neo4J
{
    using System.Collections.Generic;

    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;
    using Http.Query.Filter.Filters.Condition.Operators;

    using static System.Globalization.CultureInfo;
    using static System.Globalization.NumberStyles;
    using static System.String;

    using static Http.Query.Filter.Filters.Condition.Operators.Comparison;

    internal abstract class BaseWhere : IWhere<string, Filter>
    {
        private static readonly IReadOnlyDictionary<Comparison, string> Operations = new Dictionary<Comparison, string>
        {
            { GreaterThan, ">=" },
            { LessThan, "<=" },
            { Equal, "=" },
        };

        public abstract string Name { get; }

        public string Apply(Filter filter) => filter.HasCondition
            ? Join(" AND ", this.Apply(filter, new[] { "1=1" }))
            : "1=1";

        protected static object GetValue(object value) => value switch
        {
            string s when double.TryParse(s, Number, GetCultureInfo("en-US"), out var d) => d.ToString(CurrentCulture).Replace('.', ' ').Replace(',', '.'),
            string s => $"\"{s}\"",
            _ => value
        };

        protected static string GetOperator(Comparison comparison) => Operations[comparison];

        protected abstract IReadOnlyCollection<string> Apply(Filter filter, IReadOnlyCollection<string> initialConditions);
    }
}
