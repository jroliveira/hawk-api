namespace Hawk.Entities.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Entities.Transaction.Details;
    using Hawk.Entities.Transaction.Installment;

    public abstract class Transaction : Entity<Guid>
    {
        private readonly ICollection<Tag> tags;

        protected Transaction(Guid id, Payment.Payment payment, Account account)
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

        public virtual void AddTags(IEnumerable<Tag> newTags)
        {
            var tagsList = newTags as IList<Tag> ?? newTags.ToList();
            if (newTags == null || !tagsList.Any())
            {
                return;
            }

            tagsList
                .ToList()
                .ForEach(this.AddTag);
        }

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

        public virtual void SetId(Guid id)
        {
            this.Id = id;
        }
    }
}
