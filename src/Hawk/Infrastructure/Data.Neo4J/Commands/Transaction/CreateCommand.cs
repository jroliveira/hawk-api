namespace Hawk.Infrastructure.Data.Neo4J.Commands.Transaction
{
    using System.Linq;
    using System.Threading.Tasks;
    using Hawk.Domain.Commands.Transaction;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using static System.String;

    internal sealed class CreateCommand : ICreateCommand
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Transaction.Create.cql");
        private readonly Database database;

        public CreateCommand(Database database) => this.database = database;

        public async Task<Try<Transaction>> Execute(Transaction entity)
        {
            var statement = Statement.GetOrElse(Empty).Replace("#type#", entity.GetType().Name);

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
                tags = entity.Tags.Select(tag => tag.Name).ToArray(),
            };

            var data = await this.database.ExecuteScalar(TransactionMapping.MapFrom, statement, parameters).ConfigureAwait(false);

            return data.Lift();
        }
    }
}