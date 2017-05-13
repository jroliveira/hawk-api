namespace Finance.Infrastructure.Data.Neo4j.Commands.Tag
{
    using System.Linq;

    using Finance.Entities.Transaction;

    using global::Neo4j.Driver.V1;

    public class CreateCommand
    {
        private readonly File file;

        public CreateCommand(File file)
        {
            this.file = file;
        }

        public virtual void Execute(Transaction entity, IStatementRunner trans)
        {
            if (entity.Tags == null || !entity.Tags.Any())
            {
                return;
            }

            var query = this.file.ReadAllText(@"Tag\Create.cql");
            var parameters = new
            {
                transaction = entity.Id,
                tags = entity.Tags.Select(tag => tag.Name).ToArray()
            };

            trans.Run(query, parameters);
        }
    }
}