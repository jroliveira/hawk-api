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

    using static System.Guid;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class NewTransactionModel
    {
        private static readonly IReadOnlyDictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IReadOnlyCollection<Tag>>, Try<Transaction>>> Types =
            new Dictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IReadOnlyCollection<Tag>>, Try<Transaction>>>
            {
                { "Debit", Debit.CreateWith },
                { "Credit", Credit.CreateWith },
            };

        public NewTransactionModel(string type, PaymentModel payment, string store, IEnumerable<string> tags)
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

        public static implicit operator Option<Transaction>(NewTransactionModel model) => MapFrom(NewGuid(), model);

        public static implicit operator NewTransactionModel(Transaction entity) => new NewTransactionModel(
            entity.GetType().Name,
            entity.Payment,
            entity.Store,
            entity.Tags.Select(tag => tag.Name));

        public static Option<Transaction> MapFrom(Guid id, NewTransactionModel model)
        {
            if (!Types.TryGetValue(model.Type, out var createWith))
            {
                return None();
            }

            var tags = model
                .Tags
                .Select(tag => Tag.CreateWith(tag))
                .ToList();

            if (tags.Any(tag => tag.IsFailure))
            {
                return None();
            }

            return createWith(
                id,
                model.Payment,
                Domain.Store.Store.CreateWith(model.Store),
                new List<Tag>(tags.Select(tag => tag.Get())));
        }
    }
}
