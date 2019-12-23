namespace Hawk.Domain.Transaction
{
    using System;
    using System.Collections.Generic;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public abstract class Transaction : Entity<Guid>
    {
        protected Transaction(
            Guid id,
            Payment payment,
            Store store,
            IReadOnlyCollection<Tag> tags)
            : base(id)
        {
            this.Payment = payment;
            this.Store = store;
            this.Tags = tags;
        }

        public Payment Payment { get; }

        public Store Store { get; }

        public IReadOnlyCollection<Tag> Tags { get; }

        protected static Try<Transaction> NewTransaction(
            Option<Guid> id,
            Option<Payment> payment,
            Option<Store> store,
            Option<IReadOnlyCollection<Tag>> tags,
            Func<(Guid Id, Payment Payment, Store Store, IReadOnlyCollection<Tag> Tags), Try<Transaction>> createTransaction) =>
                id
                && payment
                && store
                && tags
                ? createTransaction((id.Get(), payment.Get(), store.Get(), tags.Get()))
                : Failure<Transaction>(new InvalidObjectException("Invalid transaction."));
    }
}
