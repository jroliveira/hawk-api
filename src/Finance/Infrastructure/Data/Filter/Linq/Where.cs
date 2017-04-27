namespace Finance.Infrastructure.Data.Filter.Linq
{
    using System.Linq;
    using System.Reflection;

    using Finance.Entities.Transaction;

    using Http.Query.Filter;

    public class Where : IWhere<bool, Filter, Transaction>
    {
        public bool Apply(Filter filter, Transaction entity)
        {
            if (!filter.HasCondition)
            {
                return true;
            }

            return filter
                .Where
                .All(field => entity
                    .GetType()
                    .GetProperty(field.Name)
                    ?.GetValue(entity).ToString() == field.Value.ToString());
        }
    }
}