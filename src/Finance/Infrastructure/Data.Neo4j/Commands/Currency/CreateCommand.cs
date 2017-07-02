namespace Finance.Infrastructure.Data.Neo4j.Commands.Currency
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
            if (string.IsNullOrWhiteSpace(entity.Payment.Currency?.Name))
            {
                return;
            }

            var query = this.file.ReadAllText(@"Currency.Create.cql");
            var parameters = new
            {
                transaction = entity.Id.ToString(),
                currency = entity.Payment.Currency.Name
            };

            trans.Run(query, parameters);
        }
    }
}