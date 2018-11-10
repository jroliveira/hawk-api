namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    using static System.String;

    internal sealed class DeleteTransaction : IDeleteTransaction
    {
        private static readonly Option<string> Statement = ReadAll("Transaction.DeleteTransaction.cql");
        private readonly Database database;

        public DeleteTransaction(Database database) => this.database = database;

        public Task<Try<Unit>> Execute(Transaction entity)
        {
            var parameters = new
            {
                email = entity.Account.Email,
                id = entity.Id.ToString(),
            };

            return this.database.Execute(Statement.GetOrElse(Empty), parameters);
        }
    }
}
