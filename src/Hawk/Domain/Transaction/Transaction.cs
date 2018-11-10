namespace Hawk.Domain.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Account;
    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.Infrastructure.Monad.Utils;

    public abstract class Transaction : Entity<Guid>
    {
        private readonly ICollection<Tag> tags = new HashSet<Tag>();

        protected Transaction(Guid id, Payment payment, Account account)
        {
            this.Id = id;
            this.Payment = payment;
            this.Account = account;
        }

        public Payment Payment { get; }

        public Account Account { get; }

        public Parcel Parcel { get; private set; }

        public Store Store { get; private set; }

        public IReadOnlyCollection<Tag> Tags => this.tags.ToList();

        public IReadOnlyCollection<Try<Unit>> AddTags(IReadOnlyCollection<Option<Tag>> tagsOption) => tagsOption
            .Select(this.AddTag)
            .ToList();

        public Try<Unit> AddTag(Option<Tag> tagOption) => this.AddTag(tagOption.GetOrElse(default));

        public Try<Unit> AddTag(Tag tag)
        {
            if (tag == null)
            {
                return new ArgumentNullException(nameof(tag), "Transaction's tag cannot be null.");
            }

            this.tags.Add(tag);

            return Util.Unit();
        }

        public Try<Unit> SplittedIn(Option<Parcel> parcelOption)
        {
            var parcel = parcelOption.GetOrElse(default);
            if (parcel == null)
            {
                return new ArgumentNullException(nameof(parcel), "Transaction's parcel cannot be null.");
            }

            this.Parcel = parcel;

            return Util.Unit();
        }

        public Try<Unit> UpdateStore(Option<Store> storeOption)
        {
            var store = storeOption.GetOrElse(default);
            if (store == null)
            {
                return new ArgumentNullException(nameof(store), "Transaction's store cannot be null.");
            }

            this.Store = store;

            return Util.Unit();
        }

        protected static Try<Transaction> CreateWith(
            Option<Guid> transactionIdOption,
            Option<Payment> paymentOption,
            Option<Account> accountOption,
            Func<Guid, Payment, Account, Try<Transaction>> createTransaction)
        {
            var payment = paymentOption.GetOrElse(default);
            if (payment == null)
            {
                return new ArgumentNullException(nameof(payment), "Transaction's payment cannot be null.");
            }

            var account = accountOption.GetOrElse(default);
            if (account == null)
            {
                return new ArgumentNullException(nameof(account), "Transaction's account cannot be null.");
            }

            var id = transactionIdOption.GetOrElse(Guid.Empty);
            if (id == Guid.Empty)
            {
                return new ArgumentNullException(nameof(id), "Transaction's id cannot be null.");
            }

            return createTransaction(id, payment, account);
        }
    }
}
