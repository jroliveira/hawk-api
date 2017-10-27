namespace Hawk.Infrastructure.Data.Neo4j.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Entities;
    using Hawk.Entities.Transaction;
    using Hawk.Entities.Transaction.Details;
    using Hawk.Infrastructure.Data.Neo4j.Extensions;
    using Hawk.Infrastructure.Data.Neo4j.Mappings.Payment;

    using global::Neo4j.Driver.V1;

    public class TransactionMapping
    {
        private readonly IDictionary<string, Func<Guid, Entities.Transaction.Payment.Payment, Account, Transaction>> types;

        private readonly AccountMapping accountMapping;
        private readonly PaymentMapping paymentMapping;
        private readonly StoreMapping storeMapping;
        private readonly ParcelMapping parcelMapping;

        public TransactionMapping(
            AccountMapping accountMapping,
            PaymentMapping paymentMapping,
            StoreMapping storeMapping,
            ParcelMapping parcelMapping)
        {
            this.accountMapping = accountMapping;
            this.paymentMapping = paymentMapping;
            this.storeMapping = storeMapping;
            this.parcelMapping = parcelMapping;

            this.types = new Dictionary<string, Func<Guid, Entities.Transaction.Payment.Payment, Account, Transaction>>
            {
                { "Debit", (id, payment, account) => new Debit(id, payment, account) },
                { "Credit", (id, payment, account) => new Credit(id, payment, account) }
            };
        }

        public Transaction MapFrom(IRecord data)
        {
            var record = data.GetRecord("data");
            var type = record
                .GetList("type")
                .FirstOrDefault(t => this.types.ContainsKey(t.ToString()));

            if (!this.types.TryGetValue(type, out var createWith))
            {
                throw new KeyNotFoundException();
            }

            var account = this.accountMapping.MapFrom(record.GetRecord("account"));
            var payment = this.paymentMapping.MapFrom(record.GetRecord("payment"));

            var transaction = createWith(
                record.GetGuid(),
                payment,
                account);

            var tags = record
                .GetList("tags")
                ?.Select(name => new Tag(name))
                .OrderBy(tag => tag.Name);
            transaction.AddTags(tags);

            transaction.Parcel = this.parcelMapping.MapFrom(record.GetRecord("parcel"));
            transaction.Store = this.storeMapping.MapFrom(record.GetRecord("store"));

            return transaction;
        }
    }
}