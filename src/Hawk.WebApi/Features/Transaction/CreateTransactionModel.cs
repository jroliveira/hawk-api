namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Hawk.Domain.Payee;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.Guid;

    using static Hawk.Domain.Payee.Payee;
    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Domain.Transaction.Expense;
    using static Hawk.Domain.Transaction.Income;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class CreateTransactionModel
    {
        private static readonly IReadOnlyDictionary<string, Func<Option<Guid>, Option<Payment>, Option<Payee>, Option<IEnumerable<Option<Tag>>>, Try<Transaction>>> Types =
            new Dictionary<string, Func<Option<Guid>, Option<Payment>, Option<Payee>, Option<IEnumerable<Option<Tag>>>, Try<Transaction>>>
            {
                { "Expense", NewExpense },
                { "Income", NewIncome },
            };

        public CreateTransactionModel(
            string type,
            PaymentModel payment,
            string payee,
            IEnumerable<string> tags)
        {
            this.Type = type;
            this.Payment = payment;
            this.Payee = payee;
            this.Tags = tags;
        }

        [Required]
        public string Type { get; }

        [Required]
        public PaymentModel Payment { get; }

        [Required]
        public string Payee { get; }

        [Required]
        public IEnumerable<string> Tags { get; }

        public static implicit operator Option<Transaction>(CreateTransactionModel model) => MapTransaction(NewGuid(), model);

        public static Option<Transaction> MapTransaction(Guid id, CreateTransactionModel model)
        {
            if (!Types.TryGetValue(model.Type, out var newTransaction))
            {
                return None();
            }

            var tags = model
                .Tags
                .Select(tag => NewTag(tag))
                .ToList();

            if (tags.Any(tag => !tag))
            {
                return None();
            }

            return newTransaction(
                id,
                model.Payment,
                NewPayee(model.Payee),
                Some(model.Tags.Select(tag => NewTag(tag).ToOption())));
        }
    }
}
