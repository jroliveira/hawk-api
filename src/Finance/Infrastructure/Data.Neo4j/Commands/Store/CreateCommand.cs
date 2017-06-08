namespace Finance.Infrastructure.Data.Neo4j.Commands.Store
{
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
            if (string.IsNullOrWhiteSpace(entity.Store?.Name))
            {
                return;
            }

            var query = this.file.ReadAllText(@"Store.Create.cql");
            var parameters = new
            {
                transaction = entity.Id,
                store = entity.Store.Name
            };

            trans.Run(query, parameters);
        }
    }
}