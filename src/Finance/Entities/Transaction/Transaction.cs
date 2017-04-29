namespace Finance.Entities.Transaction
{
    using System.Collections.Generic;

    using Finance.Entities.Transaction.Details;
    using Finance.Entities.Transaction.Installment;

    public abstract class Transaction : Entity<int>
    {
        private readonly ICollection<Tag> tags;

        protected Transaction(int id, Payment.Payment payment, Account account)
        {
            this.Id = id;
            this.Payment = payment;
            this.Account = account;
            this.tags = new List<Tag>();
        }

        public Payment.Payment Payment { get; }

        public Account Account { get; }

        public Parcel Parcel { get; set; }

        public Store Store { get; set; }

        public IEnumerable<Tag> Tags => this.tags;

        public virtual void AddTag(Tag tag)
        {
            if (tag == null)
            {
                return;
            }

            this.tags.Add(tag);
        }

        public virtual void SplittedIn(Parcel parcel)
        {
            this.Parcel = parcel;
        }

        public virtual void SetId(int id)
        {
            this.Id = id;
        }
    }
}
