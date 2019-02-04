namespace Hawk.Domain.Transaction
{
    using System;
    using System.Collections.Generic;

    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    public sealed class Debit : Transaction
    {
        private Debit(Guid id, Payment payment, Store store, IReadOnlyCollection<Tag> tags)
            : base(id, payment, store, tags)
        {
        }

        public static Try<Transaction> CreateWith(Option<Guid> id, Option<Payment> payment, Option<Store> store, Option<IReadOnlyCollection<Tag>> tags) => CreateWith(
            id,
            payment,
            store,
            tags,
            transaction => new Debit(transaction.Id, transaction.Payment, transaction.Store, transaction.Tags));
    }
}
