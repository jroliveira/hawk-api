namespace Hawk.Infrastructure.Data.Neo4j.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction;
    using Hawk.Infrastructure.Data.Neo4j.Mappings;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery : GetAllQueryBase
    {
        private readonly TransactionMapping mapping;

        public GetAllQuery(
            Database database,
            TransactionMapping mapping,
            GetScript file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, limit, skip, where)
        {
            this.mapping = mapping;
        }

        public virtual async Task<Paged<Transaction>> GetResult(string email, Filter filter)
        {
            var where = this.Where.Apply(filter, "transaction");
            var query = this.File.ReadAllText(@"Transaction.GetAll.cql");
            query = query.Replace("#where#", where);

            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Payment.Date)
                .ToList();

            return new Paged<Transaction>(entities, parameters.skip, parameters.limit);
        }
    }
}