namespace Finance.Entities.Transaction
{
    using System;
    using System.Collections.Generic;

    using Finance.Entities.Transaction.Details;
    using Finance.Entities.Transaction.Installment;

    public abstract class Transaction : Entity<int>
    {
        private readonly ICollection<Tag> tags;

        protected Transaction(decimal value, DateTime date, Account account)
            : this(new Payment.Payment(value, date), account)
        {
        }

        protected Transaction(Payment.Payment payment, Account account)
        {
            this.Payment = payment;
            this.Account = account;
            this.tags = new List<Tag>();
        }

        public Payment.Payment Payment { get; }

        public Account Account { get; set; }

        public Parcel Parcel { get; private set; }

        public Store Store { get; private set; }

        public IEnumerable<Tag> Tags => this.tags;

        public virtual void AddTag(Tag tag)
        {
            this.tags.Add(tag);
        }

        public virtual void UpdateStore(Store store)
        {
            this.Store = store;
        }

        public virtual void SplittedIn(Parcel parcel)
        {
            this.Parcel = parcel;
        }
    }
}
