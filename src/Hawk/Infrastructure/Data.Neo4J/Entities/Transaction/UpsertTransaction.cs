namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Transaction.TransactionMapping;

    using static System.String;

    internal sealed class UpsertTransaction : IUpsertTransaction
    {
        private static readonly Option<string> Statement = ReadAll("Transaction.UpsertTransaction.cql");
        private readonly Database database;

        public UpsertTransaction(Database database) => this.database = database;

        public async Task<Try<Transaction>> Execute(Transaction entity)
        {
            var statement = Statement.GetOrElse(Empty).Replace("#type#", entity.GetType().Name);

            var parameters = new
            {
                id = entity.Id.ToString(),
                email = entity.Account.Email,
                value = entity.Payment.Price.Value,
                year = entity.Payment.Date.Year,
                month = entity.Payment.Date.Month,
                day = entity.Payment.Date.Day,
                parcel = entity.Parcel?.Number,
                parcels = entity.Parcel?.Total,
                currency = entity.Payment.Price.Currency.Name,
                method = entity.Payment.PaymentMethod.Name,
                store = entity.Store.Name,
                tags = entity.Tags.Select(tag => tag.Name).ToArray(),
            };

            var data = await this.database.ExecuteScalar(MapFrom, statement, parameters).ConfigureAwait(false);

            return data.Lift();
        }
    }
}
