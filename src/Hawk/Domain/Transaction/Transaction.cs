namespace Hawk.Domain.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Category;
    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Transaction.TransactionStatus;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.Infrastructure.Uid;

    public sealed class Transaction : Entity<Guid>, IEquatable<Option<Transaction>>
    {
        private Transaction(
            Guid id,
            TransactionType type,
            TransactionStatus status,
            Option<string> description,
            Payment payment,
            Payee payee,
            Category category,
            IEnumerable<Tag> tags)
            : base(id)
        {
            this.Type = type;
            this.Payment = payment;
            this.Payee = payee;
            this.Category = category;
            this.Status = status;
            this.Description = description;
            this.Tags = tags.ToList();
        }

        public TransactionType Type { get; }

        public TransactionStatus Status { get; }

        public Option<string> Description { get; }

        public Payment Payment { get; }

        public Payee Payee { get; }

        public Category Category { get; }

        public IReadOnlyCollection<Tag> Tags { get; }

        public static Try<Transaction> NewTransaction(
            Option<TransactionType> type,
            Option<TransactionStatus> status,
            Option<string> description,
            Option<Payment> payment,
            Option<Payee> payee,
            Option<Category> category,
            Option<IEnumerable<Option<Tag>>> tags) => NewTransaction(
                id: NewGuid(),
                type,
                status,
                description,
                payment,
                payee,
                category,
                tags);

        public static Try<Transaction> NewTransaction(
            Option<Guid> id,
            Option<TransactionType> type,
            Option<TransactionStatus> status,
            Option<string> description,
            Option<Payment> payment,
            Option<Payee> payee,
            Option<Category> category,
            Option<IEnumerable<Option<Tag>>> tags) =>
                id
                && type
                && payment
                && payee
                && category
                && tags
                && tags.Get().All(_ => _)
                    ? new Transaction(
                        id.Get(),
                        type.Get(),
                        status.GetOrElse(Pending),
                        description,
                        payment.Get(),
                        payee.Get(),
                        category.Get(),
                        tags.Get().Select(tag => tag.Get()))
                    : Failure<Transaction>(new InvalidObjectException("Invalid transaction."));

        public bool Equals(Option<Transaction> other) => other.Match(
            some => this.Type == some.Type
                    && this.Payment.Equals(some.Payment)
                    && this.Payee.Equals(some.Payee)
                    && this.Category.Equals(some.Category),
            () => false);
    }
}
