namespace Finance.Infrastructure.Data.Neo4j.Commands.Transaction
{
    using Finance.Entities.Transaction;

    public class ExcludeCommand
    {
        private readonly Database database;
        private readonly GetScript file;

        public ExcludeCommand(Database database, GetScript file)
        {
            this.database = database;
            this.file = file;
        }

        public virtual void Execute(Transaction entity)
        {
            var query = this.file.ReadAllText(@"Transaction\Exclude.cql");
            var parameters = new
            {
                email = entity.Account.Email,
                id = entity.Id
            };

            this.database.Execute(session => session.Run(query, parameters));
        }
    }
}