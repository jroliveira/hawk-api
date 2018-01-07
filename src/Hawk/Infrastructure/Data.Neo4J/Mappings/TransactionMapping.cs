namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Entities.Payment;
    using Hawk.Domain.Entities.Transactions;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Infrastructure.Data.Neo4J.Mappings.Payment;

    using Neo4j.Driver.V1;

    internal sealed class TransactionMapping
    {
        private readonly IDictionary<string, Func<Guid, Pay, Account, Transaction>> types;

        private readonly AccountMapping accountMapping;
        private readonly PayMapping payMapping;
        private readonly StoreMapping storeMapping;
        private readonly ParcelMapping parcelMapping;

        public TransactionMapping(
            AccountMapping accountMapping,
            PayMapping payMapping,
            StoreMapping storeMapping,
            ParcelMapping parcelMapping)
        {
            this.accountMapping = accountMapping;
            this.payMapping = payMapping;
            this.storeMapping = storeMapping;
            this.parcelMapping = parcelMapping;

            this.types = new Dictionary<string, Func<Guid, Pay, Account, Transaction>>
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
            var payment = this.payMapping.MapFrom(record.GetRecord("payment"));

            var transaction = createWith(
                record.GetGuid(),
                payment,
                account);

            foreach (var tag in record.GetList("tags"))
            {
                transaction.AddTag(new Tag(tag));
            }

            transaction.SplittedIn(this.parcelMapping.MapFrom(record.GetRecord("parcel")));
            transaction.UpdateStore(this.storeMapping.MapFrom(record.GetRecord("store")));

            return transaction;
        }
    }
}