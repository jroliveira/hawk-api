namespace Hawk.Domain.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Category;
    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Shared.Transaction;
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
            in Guid id,
            in TransactionType type,
            in TransactionStatus status,
            in Option<string> descriptionOption,
            in Payment payment,
            in Payee payee,
            in Category category,
            in IEnumerable<Tag> tags)
            : base(id)
        {
            this.Type = type;
            this.Payment = payment;
            this.Payee = payee;
            this.Category = category;
            this.Status = status;
            this.DescriptionOption = descriptionOption;
            this.Tags = tags.ToList();
        }

        public TransactionType Type { get; }

        public TransactionStatus Status { get; }

        public Option<string> DescriptionOption { get; }

        public Payment Payment { get; }

        public Payee Payee { get; }

        public Category Category { get; }

        public IReadOnlyCollection<Tag> Tags { get; }

        public static Try<Transaction> NewTransaction(
            in Option<TransactionType> typeOption,
            in Option<TransactionStatus> statusOption,
            in Option<string> descriptionOption,
            in Option<Payment> paymentOption,
            in Option<Payee> payeeOption,
            in Option<Category> categoryOption,
            in Option<IEnumerable<Option<Tag>>> tagsOption) => NewTransaction(
                idOption: NewGuid(),
                typeOption,
                statusOption,
                descriptionOption,
                paymentOption,
                payeeOption,
                categoryOption,
                tagsOption);

        public static Try<Transaction> NewTransaction(
            in Option<Guid> idOption,
            in Option<TransactionType> typeOption,
            in Option<TransactionStatus> statusOption,
            in Option<string> descriptionOption,
            in Option<Payment> paymentOption,
            in Option<Payee> payeeOption,
            in Option<Category> categoryOption,
            in Option<IEnumerable<Option<Tag>>> tagsOption) =>
                idOption
                && typeOption
                && paymentOption
                && payeeOption
                && categoryOption
                && tagsOption
                && tagsOption.Get().All(_ => _)
                    ? new Transaction(
                        idOption.Get(),
                        typeOption.Get(),
                        statusOption.GetOrElse(Pending),
                        descriptionOption,
                        paymentOption.Get(),
                        payeeOption.Get(),
                        categoryOption.Get(),
                        tagsOption.Get().Select(tag => tag.Get()))
                    : Failure<Transaction>(new InvalidObjectException($"Invalid transaction '{idOption.GetStringOrElse("undefined")}'."));

        public bool Equals(Option<Transaction> otherOption) => otherOption
            .Fold(false)(other => this.Type == other.Type
                                  && this.Payment.Equals(other.Payment)
                                  && this.Payee.Equals(other.Payee)
                                  && this.Category.Equals(other.Category));
    }
}
