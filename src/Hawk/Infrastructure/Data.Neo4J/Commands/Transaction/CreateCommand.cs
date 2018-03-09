namespace Hawk.Infrastructure.Data.Neo4J.Commands.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Transaction;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class CreateCommand : Connection, ICreateCommand
    {
        private readonly TransactionMapping mapping;

        public CreateCommand(Database database, GetScript file, TransactionMapping mapping)
            : base(database, file, "Transaction.Create.cql")
        {
            Guard.NotNull(mapping, nameof(mapping), "Transaction mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Transaction> Execute(Transaction entity)
        {
            var statement = this.Statement.Replace("#type#", entity.GetType().Name);

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

            var inserted = await this.Database.Execute(this.mapping.MapFrom, statement, parameters).ConfigureAwait(false);

            return inserted.FirstOrDefault();
        }
    }
}