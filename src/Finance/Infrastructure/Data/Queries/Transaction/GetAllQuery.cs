namespace Finance.Infrastructure.Data.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Collections;
    using Finance.Infrastructure.Data.Filter;

    using Http.Query.Filter;

    public class GetAllQuery
    {
        private readonly ISkip<int, Filter> skip;
        private readonly ILimit<int, Filter> limit;
        private readonly IWhere<bool, Filter, Transaction> where;

        public GetAllQuery(
            ISkip<int, Filter> skip,
            ILimit<int, Filter> limit,
            IWhere<bool, Filter, Transaction> where)
        {
            this.skip = skip;
            this.limit = limit;
            this.where = where;
        }

        public virtual async Task<Paged<Transaction>> GetResultAsync(Filter filter)
        {
            var data = Transactions
                .Data
                .Skip(this.skip.Apply(filter))
                .Take(this.limit.Apply(filter))
                .Where(transaction => this.where.Apply(filter, transaction))
                .ToList();

            return await Task.Run(() => new Paged<Transaction>(
                data,
                filter.Skip,
                filter.Limit));
        }
    }
}