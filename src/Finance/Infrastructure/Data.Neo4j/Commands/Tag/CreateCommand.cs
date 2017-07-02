namespace Finance.Infrastructure.Data.Neo4j.Commands.Tag
{
    using System.Linq;

    using Finance.Entities.Transaction;

    using global::Neo4j.Driver.V1;

    public class CreateCommand
    {
        private readonly GetScript file;

        public CreateCommand(GetScript file)
        {
            this.file = file;
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