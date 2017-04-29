namespace Finance.Infrastructure.Data.Neo4j.Commands.Transaction
{
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Finance.Entities.Transaction;

    using global::Neo4j.Driver.V1;

    public class CreateCommand
    {
        private readonly Database database;
        private readonly File file;

        public CreateCommand(Database database, File file)
        {
            this.database = database;
            this.file = file;
        }

        public virtual async Task<Transaction> ExecuteAsync(Transaction entity)
        {
            var query = this.file.ReadAllText(@"Transaction\create.cql");
            var parameters = new
            {
                type = entity.GetType().Name,
                email = entity.Account.Email,
                value = entity.Payment.Value,
                date = entity.Payment.Date.Date.ToString(CultureInfo.InvariantCulture),
                paymentMethod = entity.Payment.Method.Name,
                store = entity.Store.Name,
                parcel = entity.Parcel.Number,
                parcels = entity.Parcel.Total,
                tags = entity.Tags.Select(tag => tag.Name).ToArray()
            };

            var id = await this.database.ExecuteAsync(async session => await Task.Run(() =>
                session
                    .Run(query, parameters)
                    .Select(record => record["id"].As<int>())
                    .FirstOrDefault()));

            entity.SetId(id);
            return entity;
        }
    }
}