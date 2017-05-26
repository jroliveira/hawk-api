namespace Finance.Infrastructure.Data.Neo4j.Commands.Transaction
{
    using System.Globalization;
    using System.Linq;

    using Finance.Entities.Transaction;

    using global::Neo4j.Driver.V1;

    public class CreateCommand
    {
        private readonly Currency.CreateCommand createCurrency;
        private readonly PaymentMethod.CreateCommand createPaymentMethod;
        private readonly Store.CreateCommand createStore;
        private readonly Tag.CreateCommand createTag;

        private readonly Database database;
        private readonly GetScript file;

        public CreateCommand(
            Currency.CreateCommand createCurrency,
            PaymentMethod.CreateCommand createPaymentMethod,
            Store.CreateCommand createStore,
            Tag.CreateCommand createTag,
            Database database,
            GetScript file)
        {
            this.createCurrency = createCurrency;
            this.createPaymentMethod = createPaymentMethod;
            this.createStore = createStore;
            this.createTag = createTag;
            this.database = database;
            this.file = file;
        }

        public virtual Transaction Execute(Transaction entity)
        {
            var query = this.file.ReadAllText(@"Transaction\Create.cql");
            var parameters = new
            {
                type = entity.GetType().Name,
                email = entity.Account.Email,
                value = entity.Payment.Value,
                date = entity.Payment.Date.Date.ToString(CultureInfo.InvariantCulture),
                parcel = entity.Parcel?.Number,
                parcels = entity.Parcel?.Total
            };

            return this.database.Execute(session =>
            {
                using (var trans = session.BeginTransaction())
                {
                    var id = trans
                        .Run(query, parameters)
                        .Select(record => record["id"].As<int>())
                        .FirstOrDefault();

                    entity.SetId(id);

                    this.createCurrency.Execute(entity, trans);
                    this.createPaymentMethod.Execute(entity, trans);
                    this.createStore.Execute(entity, trans);
                    this.createTag.Execute(entity, trans);

                    trans.Success();
                    return entity;
                }
            });
        }
    }
}