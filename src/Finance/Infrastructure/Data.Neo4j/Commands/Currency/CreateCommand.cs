namespace Finance.Infrastructure.Data.Neo4j.Commands.Currency
{
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
            if (entity.Payment.Currency == null)
            {
                return;
            }

            var query = this.file.ReadAllText(@"Currency\Create.cql");
            var parameters = new
            {
                transaction = entity.Id,
                currency = entity.Payment.Currency.Name
            };

            trans.Run(query, parameters);
        }
    }
}