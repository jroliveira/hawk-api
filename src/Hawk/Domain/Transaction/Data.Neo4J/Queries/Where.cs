namespace Hawk.Domain.Transaction.Data.Neo4J.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Filter.Data.Neo4J;
    using Hawk.Infrastructure.Monad.Extensions;

    using Http.Query.Filter;

    using static System.String;

    using static Hawk.Domain.Category.Category;
    using static Hawk.Domain.Currency.Currency;
    using static Hawk.Domain.Payee.Payee;
    using static Hawk.Domain.PaymentMethod.PaymentMethod;
    using static Hawk.Domain.Tag.Tag;

    internal sealed class Where : BaseWhere
    {
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

        private static string GetCondition(
            in string field,
            in string @operator,
            in object value)
        {
            switch (field)
            {
                case "year":
                case "month":
                case "day":
                case "value":
                case "description":
                    return $"transaction.{field} {@operator} {value}";
                case "status":
                    return $"transaction.{field} {@operator} {value.ToString().ToPascalCase()}";
                case "type":
                    return $"{value} IN labels(transaction)";
                case "category":
                    return $"EXISTS {{ MATCH(transaction)-[:WITH]->(n:Category) WHERE n.name {@operator} {NewCategory(value.ToString()).GetStringOrElse(Empty)} }}";
                case "tag":
                    return $"EXISTS {{ MATCH(transaction)-[:HAS]->(n:Tag) WHERE n.name {@operator} {NewTag(value.ToString()).GetStringOrElse(Empty)} }}";
                case "currency":
                    return $"EXISTS {{ MATCH(transaction)-[:PAID_IN]->(n:Currency) WHERE n.code {@operator} {NewCurrency(value.ToString()).GetStringOrElse(Empty)} }}";
                case "payee":
                    return $"EXISTS {{ MATCH(transaction)-[:IN]->(n:Payee) WHERE n.name {@operator} {NewPayee(value.ToString()).GetStringOrElse(Empty)} }}";
                case "paymentmethod":
                    return $"EXISTS {{ MATCH(transaction)-[:PAID_WITH]->(n:PaymentMethod) WHERE n.name {@operator} {NewPaymentMethod(value.ToString()).GetStringOrElse(Empty)} }}";
                default:
                    return "1=1";
            }
        }
    }
}
