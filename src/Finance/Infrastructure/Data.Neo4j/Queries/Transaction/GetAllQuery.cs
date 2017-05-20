namespace Finance.Infrastructure.Data.Neo4j.Queries.Transaction
{
    using System.Linq;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Mappings;
    using Finance.Infrastructure.Filter;

    using Http.Query.Filter;

    public class GetAllQuery : GetAllQueryBase
    {
        private readonly TransactionMapping mapping;

        public GetAllQuery(
            Database database,
            TransactionMapping mapping,
            File file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, limit, skip, where)
        {
            this.mapping = mapping;
        }

        public virtual Paged<Transaction> GetResult(string email, Filter filter)
        {
            var query = this.File.ReadAllText(@"Transaction\GetAll.cql");
            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = this.Database.Execute(this.mapping.MapFrom, query, parameters);
            var entities = data
                .OrderBy(item => item.Payment.Date)
                .ThenBy(item => item.Id)
                .ToList();

            return new Paged<Transaction>(entities, parameters.skip, parameters.limit);
        }
    }
}