namespace Hawk.Infrastructure.Data.Neo4j.Commands.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction;

    using global::Neo4j.Driver.V1;

    public class CreateCommand
    {
        private readonly GetScript file;

        public CreateCommand(GetScript file)
        {
            this.file = file;
        }

        public virtual async Task Execute(Transaction entity, IStatementRunner trans)
        {
            if (entity?.Payment?.Method == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(entity.Payment.Method?.Name))
            {
                return;
            }

            var query = this.file.ReadAllText(@"PaymentMethod.Create.cql");
            var parameters = new
            {
                transaction = entity.Id.ToString(),
                method = entity.Payment.Method.Name
            };

            await trans.RunAsync(query, parameters).ConfigureAwait(false);
        }
    }
}