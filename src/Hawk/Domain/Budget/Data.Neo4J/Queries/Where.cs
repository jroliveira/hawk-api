namespace Hawk.Domain.Budget.Data.Neo4J.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Filter.Data.Neo4J;
    using Hawk.Infrastructure.Monad.Extensions;

    using Http.Query.Filter;

    using static System.String;

    using static Hawk.Domain.Category.Category;

    internal sealed class Where : BaseWhere
    {
        public override string Name => typeof(Where).FullName!;

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
                case "limit":
                case "interval":
                case "description":
                    return $"budget.{field} {@operator} {value}";
                case "month":
                case "year":
                    return $"budget.{field} <= {value}";
                case "frequency":
                    return $"budget.{field} {@operator} {value.ToString().ToPascalCase()}";
                case "category":
                    return $"EXISTS {{ MATCH(budget)-[:HAS]->(n:Category) WHERE n.name {@operator} {NewCategory(value.ToString()).GetStringOrElse(Empty)} }}";
                default:
                    return "1=1";
            }
        }
    }
}
