namespace Hawk.Infrastructure.Data.Neo4J.Queries.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;
    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static System.String;

    internal sealed class GetByIdQuery : IGetByIdQuery
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Transaction.GetById.cql");
        private readonly Database database;

        public GetByIdQuery(Database database) => this.database = database;

        public async Task<Try<Option<Transaction>>> GetResult(string id, string email)
        {
            var parameters = new
            {
                id,
                email,
            };

            var data = await this.database.ExecuteScalar(TransactionMapping.MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.SelectMany(Some);
        }
    }
}