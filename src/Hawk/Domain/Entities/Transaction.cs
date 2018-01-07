namespace Hawk.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Entities.Payment;

    public abstract class Transaction : Entity<Guid>
    {
        private readonly ICollection<Tag> tags;

        protected Transaction(Guid id, Pay pay, Account account)
        {
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
            if (tag == null)
            {
                return;
            }

            this.tags.Add(tag);
        }

        public void SplittedIn(Parcel parcel)
        {
            this.Parcel = parcel;
        }

        public void UpdateStore(Store store)
        {
            this.Store = store;
        }

        public Transaction Clone(Guid id)
        {
            this.Id = id;
            return this;
        }
    }
}
