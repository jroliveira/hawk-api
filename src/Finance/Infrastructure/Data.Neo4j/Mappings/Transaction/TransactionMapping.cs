namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Finance.Entities;
    using Finance.Entities.Transaction;
    using Finance.Entities.Transaction.Details;
    using Finance.Infrastructure.Data.Neo4j.Extensions;
    using Finance.Infrastructure.Data.Neo4j.Mappings.Transaction.Payment;

    using global::Neo4j.Driver.V1;

    public class TransactionMapping : IMapping<Transaction>
    {
        private readonly IDictionary<string, Func<int, Entities.Transaction.Payment.Payment, Account, Transaction>> types;

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

            this.types = new Dictionary<string, Func<int, Entities.Transaction.Payment.Payment, Account, Transaction>>
            {
                { "Debit", (id, payment, account) => new Debit(id, payment, account) },
                { "Credit", (id, payment, account) => new Credit(id, payment, account) }
            };
        }

        public Transaction MapFrom(IRecord record)
        {
            return this.MapFrom(record.GetRecord("data"));
        }

        public Transaction MapFrom(Record record)
        {
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
                record.Get<int>("id"),
                payment,
                account);

            var tags = record
                .GetList("tags")
                ?.Select(name => new Tag(name));
            transaction.AddTags(tags);

            transaction.Parcel = this.parcelMapping.MapFrom(record.GetRecord("parcel"));
            transaction.Store = this.storeMapping.MapFrom(record.GetRecord("store"));

            return transaction;
        }
    }
}