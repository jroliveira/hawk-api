namespace Hawk.Infrastructure.Data.Neo4J.Commands.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Transaction;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class CreateCommand : ICreateCommand
    {
        private readonly Database database;
        private readonly TransactionMapping mapping;
        private readonly GetScript file;

        public CreateCommand(Database database, TransactionMapping mapping, GetScript file)
        {
            Guard.NotNull(database, nameof(database), "Database cannot be null.");
            Guard.NotNull(mapping, nameof(mapping), "Transaction mapping cannot be null.");
            Guard.NotNull(file, nameof(file), "Get script cannot be null.");

            this.database = database;
            this.mapping = mapping;
            this.file = file;
        }

        public async Task<Transaction> Execute(Transaction entity)
        {
            var query = this.file.ReadAllText(@"Transaction.Create.cql");
            query = query.Replace("#type#", entity.GetType().Name);

            var parameters = new
            {
                id = entity.Id.ToString(),
                email = entity.Account.Email,
                value = entity.Pay.Price.Value,
                year = entity.Pay.Date.Year,
                month = entity.Pay.Date.Month,
                day = entity.Pay.Date.Day,
                parcel = entity.Parcel?.Number,
                parcels = entity.Parcel?.Total,
                currency = entity.Pay.Price.Currency.Name,
                method = entity.Pay.Method.Name,
                store = entity.Store.Name,
                tags = entity.Tags.Select(tag => tag.Name).ToArray()
            };

            var inserted = await this.database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return inserted.FirstOrDefault();
        }
    }
}