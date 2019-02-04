namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Transaction.TransactionMapping;

    internal sealed class GetTransactionById : IGetTransactionById
    {
        private static readonly Option<string> Statement = ReadAll("Transaction.GetTransactionById.cql");
        private readonly Database database;

        public GetTransactionById(Database database) => this.database = database;

        public Task<Try<Transaction>> GetResult(Email email, Guid id)
        {
            var parameters = new
            {
                email = email.ToString(),
                id = id.ToString(),
            };

            return this.database.ExecuteScalar(MapFrom, Statement, parameters);
        }
    }
}
