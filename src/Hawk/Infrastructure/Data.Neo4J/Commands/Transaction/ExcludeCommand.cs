namespace Hawk.Infrastructure.Data.Neo4J.Commands.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Transaction;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;

    internal sealed class ExcludeCommand : IExcludeCommand
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Transaction.Exclude.cql");
        private readonly Database database;

        public ExcludeCommand(Database database) => this.database = database;

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