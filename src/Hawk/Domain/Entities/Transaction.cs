namespace Hawk.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure;

    public abstract class Transaction : Entity<Guid>
    {
        private readonly ICollection<Tag> tags;

        protected Transaction(Guid id, Pay pay, Account account)
        {
            Guard.NotNull(pay, nameof(pay), "Pay cannot be null.");
            Guard.NotNull(account, nameof(account), "Account cannot be null.");

            this.Id = id;
            this.Pay = pay;
            this.Account = account;
            this.tags = new List<Tag>();
        }

        public Pay Pay { get; }

        public Account Account { get; }

        public Parcel Parcel { get; private set; }

        public Store Store { get; private set; }

        public IReadOnlyCollection<Tag> Tags => this.tags.ToList();

        public void AddTag(Tag tag)
        {
            Guard.NotNull(tag, nameof(tag), "Tag cannot be null.");

            this.tags.Add(tag);
        }

        public void SplittedIn(Parcel parcel)
        {
            Guard.NotNull(parcel, nameof(parcel), "Parcel cannot be null.");

            this.Parcel = parcel;
        }

        public void UpdateStore(Store store)
        {
            Guard.NotNull(store, nameof(store), "Store cannot be null.");

            this.Store = store;
        }

        public Transaction Clone(Guid id)
        {
            this.Id = id;
            return this;
        }
    }
}
