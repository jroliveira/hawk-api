namespace Finance.Infrastructure.Data.Neo4j.Commands.Tag
{
    using System.Linq;

    using Finance.Entities;
    using Finance.Entities.Transaction;
    using Finance.Entities.Transaction.Details;

    using global::Neo4j.Driver.V1;

    public class CreateCommand
    {
        private readonly GetScript file;
        private readonly Database database;

        public CreateCommand(GetScript file, Database database)
        {
            this.file = file;
            this.database = database;
        }

        public virtual void Execute(Tag tag, Account account)
        {
            var query = this.file.ReadAllText(@"Tag.Create.cql");
            var parameters = new
            {
                email = account.Email,
                tags = tag.Name
            };

            this.database.Execute(session => session.Run(query, parameters));
        }

        public virtual void Execute(Transaction entity, IStatementRunner trans)
        {
            var query = this.file.ReadAllText(@"Tag.Create.cql");
            var parameters = new
            {
                transaction = entity.Id.ToString(),
                tags = entity.Tags.Select(tag => tag.Name).ToArray()
            };

            trans.Run(query, parameters);
        }
    }
}