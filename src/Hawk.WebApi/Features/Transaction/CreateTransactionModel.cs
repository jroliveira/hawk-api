namespace Hawk.WebApi.Features.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.Guid;

    using static Hawk.Domain.Store.Store;
    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Domain.Transaction.Credit;
    using static Hawk.Domain.Transaction.Debit;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class CreateTransactionModel
    {
        private static readonly IReadOnlyDictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IEnumerable<Option<Tag>>>, Try<Transaction>>> Types =
            new Dictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IEnumerable<Option<Tag>>>, Try<Transaction>>>
            {
                { "Debit", NewDebit },
                { "Credit", NewCredit },
            };

        public CreateTransactionModel(
            string type,
            PaymentModel payment,
            string store,
            IEnumerable<string> tags)
        {
            this.Type = type;
            this.Payment = payment;
            this.Store = store;
            this.Tags = tags;
        }

        [Required]
        public string Type { get; }

        [Required]
        public PaymentModel Payment { get; }

        [Required]
        public string Store { get; }

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
                NewStore(model.Store),
                Some(model.Tags.Select(tag => NewTag(tag).ToOption())));
        }
    }
}
