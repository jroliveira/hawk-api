namespace Hawk.Infrastructure.Data.Neo4J.Commands.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Transaction;
    using Hawk.Domain.Entities;

    internal sealed class ExcludeCommand : Connection, IExcludeCommand
    {
        public ExcludeCommand(Database database, GetScript file)
            : base(database, file, "Transaction.Exclude.cql")
        {
        }

        public async Task Execute(Transaction entity)
        {
            var parameters = new
            {
                email = entity.Account.Email,
                id = entity.Id.ToString()
            };

            await this.Database.Execute(this.Statement, parameters).ConfigureAwait(false);
        }
    }
}