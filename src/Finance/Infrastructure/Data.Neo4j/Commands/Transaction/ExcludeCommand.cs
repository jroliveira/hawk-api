namespace Finance.Infrastructure.Data.Neo4j.Commands.Transaction
{
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;

    public class ExcludeCommand
    {
        private readonly Database database;
        private readonly File file;

        public ExcludeCommand(Database database, File file)
        {
            this.database = database;
            this.file = file;
        }

        public virtual async Task ExecuteAsync(Transaction entity)
        {
            var query = this.file.ReadAllText(@"Transaction\exclude.cql");
            var parameters = new
            {
                email = entity.Account.Email,
                id = entity.Id
            };

            this.database.Execute(session => session.Run(query, parameters));
        }
    }
}