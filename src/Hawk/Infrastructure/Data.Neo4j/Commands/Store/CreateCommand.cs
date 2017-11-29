namespace Hawk.Infrastructure.Data.Neo4j.Commands.Store
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
            if (entity?.Store == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(entity.Store?.Name))
            {
                return;
            }

            var query = this.file.ReadAllText(@"Store.Create.cql");
            var parameters = new
            {
                transaction = entity.Id.ToString(),
                store = entity.Store.Name
            };

            await trans.RunAsync(query, parameters).ConfigureAwait(false);
        }
    }
}