namespace Hawk.Infrastructure.Data.Neo4J.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class GetByIdQuery : QueryBase, IGetByIdQuery
    {
        private readonly TransactionMapping mapping;

        public GetByIdQuery(Database database, TransactionMapping mapping, GetScript file)
            : base(database, file)
        {
            Guard.NotNull(mapping, nameof(mapping), "Transaction mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Transaction> GetResult(string id, string email)
        {
            var query = this.File.ReadAllText(@"Transaction.GetById.cql");
            var parameters = new
            {
                id,
                email
            };

            var entities = await this.Database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return entities.FirstOrDefault();
        }
    }
}