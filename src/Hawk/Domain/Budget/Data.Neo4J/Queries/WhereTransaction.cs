namespace Hawk.Domain.Budget.Data.Neo4J.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.Filter.Data.Neo4J;

    using Http.Query.Filter;

    internal sealed class WhereTransaction : BaseWhere
    {
        public override string Name => typeof(WhereTransaction).FullName!;

        protected override IReadOnlyCollection<string> Apply(Filter filter, IReadOnlyCollection<string> initialConditions)
        {
            var result = new List<string>(initialConditions);

            result.AddRange(filter
                .Where
                .Select(condition => GetCondition(
                    condition.Field.ToLower(),
                    GetOperator(condition.Comparison),
                    GetValue(condition.Value))));

            return result.ToList();
        }

        private static string GetCondition(string field, string @operator, object value)
        {
            switch (field)
            {
                case "month":
                case "year":
                    return $"transaction.{field} {@operator} {value}";
                default:
                    return "1=1";
            }
        }
    }
}
