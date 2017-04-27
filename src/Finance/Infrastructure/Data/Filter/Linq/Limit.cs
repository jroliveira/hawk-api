namespace Finance.Infrastructure.Data.Filter.Linq
{
    using Http.Query.Filter;

    public class Limit : ILimit<int, Filter>
    {
        public int Apply(Filter filter)
        {
            if (filter.Limit == null)
            {
                return 100;
            }

            if (filter.Limit < 1)
            {
                return 100;
            }

            return filter.Limit;
        }
    }
}
