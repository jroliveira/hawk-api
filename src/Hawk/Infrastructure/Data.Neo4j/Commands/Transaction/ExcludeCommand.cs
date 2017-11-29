namespace Hawk.Infrastructure.Data.Neo4j.Commands.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction;

    public class ExcludeCommand
    {
        private readonly Database database;
        private readonly GetScript file;

        public ExcludeCommand(Database database, GetScript file)
        {
            this.database = database;
            this.file = file;
        }

        public virtual async Task Execute(Transaction entity)
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