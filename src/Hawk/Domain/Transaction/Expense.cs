namespace Hawk.Domain.Transaction
{
    using System;
    using System.Collections.Generic;

    using Hawk.Domain.Payee;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    public sealed class Expense : Transaction
    {
        private Expense(Guid id, Payment payment, Payee payee, IEnumerable<Tag> tags)
            : base(id, payment, payee, tags)
        {
        }

        public override string Type => "Expense";

        public static Try<Transaction> NewExpense(
            Option<Guid> id,
            Option<Payment> payment,
            Option<Payee> payee,
            Option<IEnumerable<Option<Tag>>> tags) => NewTransaction(
                id,
                payment,
                payee,
                tags,
                transaction => new Expense(transaction.Id, transaction.Payment, transaction.Payee, transaction.Tags));
    }
}
