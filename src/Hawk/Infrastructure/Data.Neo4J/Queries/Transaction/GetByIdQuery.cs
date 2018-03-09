namespace Hawk.Infrastructure.Data.Neo4J.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class GetByIdQuery : Connection, IGetByIdQuery
    {
        private readonly TransactionMapping mapping;

        public GetByIdQuery(Database database, GetScript file, TransactionMapping mapping)
            : base(database, file, "Transaction.GetById.cql")
        {
            Guard.NotNull(mapping, nameof(mapping), "Transaction mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Transaction> GetResult(string id, string email)
        {
            var parameters = new
            {
                id,
                email
            };

            var entities = await this.Database.Execute(this.mapping.MapFrom, this.Statement, parameters).ConfigureAwait(false);

            return entities.FirstOrDefault();
        }
    }
}