namespace Hawk.Domain.Installment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.Convert;
    using static System.Guid;

    using static Hawk.Domain.Transaction.Payment;
    using static Hawk.Domain.Transaction.Transaction;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Installment : Entity<Guid>
    {
        private Installment(
            in Guid id,
            in InstallmentFrequency frequency,
            in Installments installments)
            : base(id)
        {
            this.Frequency = frequency;
            this.Installments = installments;
        }

        public InstallmentFrequency Frequency { get; }

        public Installments Installments { get; set; }

        public static implicit operator Guid(Installment entity) => entity.Id;

        public static Try<Installment> NewInstallment(
            in Option<InstallmentFrequency> frequencyOption,
            in Option<Installments> installmentsOption) => NewInstallment(
                idOption: NewGuid(),
                frequencyOption,
                installmentsOption);

        public static Try<Installment> NewInstallment(
            in Option<Guid> idOption,
            in Option<InstallmentFrequency> frequencyOption,
            in Option<Installments> installmentsOption) =>
                idOption
                && frequencyOption
                && installmentsOption
                    ? new Installment(
                        idOption.Get(),
                        frequencyOption.Get(),
                        installmentsOption.Get())
                    : Failure<Installment>(new InvalidObjectException("Invalid installment."));

        public Try<IEnumerable<Option<Transaction>>> GenerateTransactions(Option<Transaction> firstTransactionOption) => firstTransactionOption
            .Fold(Failure<IEnumerable<Option<Transaction>>>(new InvalidObjectException("Invalid installment.")))(firstTransaction => Success(this.Installments.Total.ForEach(
                starting: 2,
                addItem: (current, index) => NewTransaction(
                    firstTransaction.Type,
                    firstTransaction.DescriptionOption,
                    NewPayment(
                        firstTransaction.Payment.Cost,
                        firstTransaction.Payment.Date.AddMonths(ToInt32(index)),
                        firstTransaction.Payment.PaymentMethod),
                    firstTransaction.Payee,
                    firstTransaction.Category,
                    this,
                    Some(firstTransaction.Tags.Select(tag => Some(tag)))).ToOption())));
    }
}
