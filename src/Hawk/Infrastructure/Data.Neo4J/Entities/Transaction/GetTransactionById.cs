namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Transaction.TransactionMapping;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static System.String;

    internal sealed class GetTransactionById : IGetTransactionById
    {
        private static readonly Option<string> Statement = ReadAll("Transaction.GetTransactionById.cql");
        private readonly Database database;

        public GetTransactionById(Database database) => this.database = database;

        public async Task<Try<Option<Transaction>>> GetResult(string id, string email)
        {
            var parameters = new
            {
                id,
                email,
            };

            var data = await this.database.ExecuteScalar(MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.SelectMany(Some);
        }
    }
}
