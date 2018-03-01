namespace Hawk.Infrastructure.Data.Neo4J.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    internal sealed class GetAllQuery : GetAllQueryBase, IGetAllQuery
    {
        private readonly TransactionMapping mapping;

        public GetAllQuery(
            Database database,
            TransactionMapping mapping,
            GetScript file,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip,
            IWhere<string, Filter> where)
            : base(database, file, "Transaction.GetAll.cql", limit, skip, where)
        {
            Guard.NotNull(mapping, nameof(mapping), "Transaction mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Paged<Transaction>> GetResult(string email, Filter filter)
        {
            var where = this.Where.Apply(filter, "transaction");
            var statement = this.Statement;
            statement = statement.Replace("#where#", where);

            var parameters = new
            {
                email,
                skip = this.Skip.Apply(filter),
                limit = this.Limit.Apply(filter)
            };

            var data = await this.Database.Execute(this.mapping.MapFrom, statement, parameters).ConfigureAwait(false);
            var entities = data
                .OrderBy(item => item.Pay.Date)
                .ToList();

            return new Paged<Transaction>(entities, parameters.skip, parameters.limit);
        }
    }
}