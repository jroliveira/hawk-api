namespace Finance.Infrastructure.Data.Neo4j.Filter
{
    using System.Collections.Generic;
    using System.Linq;

    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;
    using Http.Query.Filter.Filters.Condition;
    using Http.Query.Filter.Filters.Condition.Operators;

    public class Where : IWhere<string, Filter>
    {
        public string Apply(Filter filter)
        {
            if (!filter.HasCondition)
            {
                return "1 = 1";
            }

            var condition = filter.Where.FirstOrDefault();

            return $"{condition.Name} {GetOperator(condition)} {condition.Value}";
        }

        private static string GetOperator(Field field)
        {
            var operations = new Dictionary<Comparison, string>
            {
                { Comparison.GreaterThan, ">=" },
                { Comparison.LessThan, "<=" },
                { Comparison.Equal, "=" }
            };

            return operations[field.Comparison];
        }
    }
}
