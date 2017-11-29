namespace Hawk.Infrastructure.Data.Neo4j.Commands.Transaction
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities.Transaction;

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

        public virtual async Task<Transaction> Execute(Transaction entity)
        {
            var query = this.file.ReadAllText(@"Transaction.Create.cql");
            query = query.Replace("#type#", entity.GetType().Name);
            var parameters = new
            {
                id = entity.Id.ToString(),
                email = entity.Account.Email,
                value = entity.Payment.Value,
                year = entity.Payment.Date.Year,
                month = entity.Payment.Date.Month,
                day = entity.Payment.Date.Day,
                parcel = entity.Parcel?.Number,
                parcels = entity.Parcel?.Total
            };

            return await this.database.Execute(async session =>
            {
                using (var trans = session.BeginTransaction())
                {
                    var cursor = await trans.RunAsync(query, parameters).ConfigureAwait(false);
                    var data = await cursor.ToListAsync().ConfigureAwait(false);
                    var id = data.Select(record => new Guid(record["id"].As<string>())).FirstOrDefault();

                    entity.SetId(id);

                    await this.createCurrency.Execute(entity, trans).ConfigureAwait(false);
                    await this.createPaymentMethod.Execute(entity, trans).ConfigureAwait(false);
                    await this.createStore.Execute(entity, trans).ConfigureAwait(false);
                    await this.createTag.Execute(entity, trans).ConfigureAwait(false);

                    trans.Success();
                    return entity;
                }
            }).ConfigureAwait(false);
        }
    }
}