namespace Finance.Infrastructure.Data.Neo4j.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Mappings.Transaction;
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery
    {
        private readonly Database database;
        private readonly TransactionMapping mapping;
        private readonly File file;

        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;
        private readonly IWhere<string, Filter> where;

        public GetAllQuery(
            Database database,
            TransactionMapping mapping,
            File file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
        {
            this.database = database;
            this.mapping = mapping;
            this.file = file;
            this.limit = limit;
            this.skip = skip;
            this.where = where;
        }

        public virtual async Task<Paged<Transaction>> GetResultAsync(string email, Filter filter)
        {
            var query = this.file.ReadAllText(@"Transaction\get-all.cql");
            var parameters = new
            {
                email,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter)
            };

            var data = this.database.Execute(this.mapping.MapFrom, query, parameters);
            var entities = data
                .OrderBy(item => item.Payment.Date)
                .ThenBy(item => item.Id)
                .ToList();

            return new Paged<Transaction>(entities, parameters.skip, parameters.limit);
        }
    }
}