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
            var transactionQuery = this.file.ReadAllText(@"Transaction\create.cql");
            var transactionData = new
            {
                type = entity.GetType().Name,
                email = entity.Account.Email,
                value = entity.Payment.Value,
                date = entity.Payment.Date.Date.ToString(CultureInfo.InvariantCulture),
                parcel = entity.Parcel?.Number,
                parcels = entity.Parcel?.Total
            };

            return await this.database.ExecuteAsync(async session => await Task.Run(() =>
            {
                using (var trans = session.BeginTransaction())
                {
                    var id = trans
                        .Run(transactionQuery, transactionData)
                        .Select(record => record["id"].As<int>())
                        .FirstOrDefault();

                    entity.SetId(id);

                    this.CreatePaymentMethod(trans, entity);
                    this.CreateStore(trans, entity);
                    this.CreateTags(trans, entity);

                    trans.Success();
                    return entity;
                }
            }));
        }

        private void CreatePaymentMethod(IStatementRunner trans, Transaction entity)
        {
            if (entity.Payment.Method == null)
            {
                return;
            }

            var query = this.file.ReadAllText(@"Transaction\Payment\create-method.cql");
            var parameters = new
            {
                transaction = entity.Id,
                method = entity.Payment.Method.Name
            };

            trans.Run(query, parameters);
        }

        private void CreateStore(IStatementRunner trans, Transaction entity)
        {
            if (entity.Store == null)
            {
                return;
            }

            var query = this.file.ReadAllText(@"Transaction\Details\create-store.cql");
            var parameters = new
            {
                transaction = entity.Id,
                store = entity.Store.Name
            };

            trans.Run(query, parameters);
        }

        private void CreateTags(IStatementRunner trans, Transaction entity)
        {
            if (entity.Tags == null || !entity.Tags.Any())
            {
                return;
            }

            var query = this.file.ReadAllText(@"Transaction\Details\create-tags.cql");
            var parameters = new
            {
                transaction = entity.Id,
                tags = entity.Tags.Select(tag => tag.Name).ToArray()
            };

            trans.Run(query, parameters);
        }
    }
}