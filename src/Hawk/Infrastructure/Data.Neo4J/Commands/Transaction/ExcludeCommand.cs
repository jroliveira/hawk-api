namespace Hawk.Infrastructure.Data.Neo4J.Commands.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Transaction;
    using Hawk.Domain.Entities;

    internal sealed class ExcludeCommand : IExcludeCommand
    {
        private readonly Database database;
        private readonly GetScript file;

        public ExcludeCommand(Database database, GetScript file)
        {
            Guard.NotNull(database, nameof(database), "Database cannot be null.");
            Guard.NotNull(file, nameof(file), "Get script cannot be null.");

            this.database = database;
            this.file = file;
        }

        public async Task Execute(Transaction entity)
        {
            var query = this.file.ReadAllText(@"Transaction.Exclude.cql");
            var parameters = new
            {
                email = entity.Account.Email,
                id = entity.Id.ToString()
            };

            await this.database.Execute(async session => await session.RunAsync(query, parameters).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}