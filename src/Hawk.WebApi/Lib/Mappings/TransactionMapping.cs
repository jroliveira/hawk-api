namespace Hawk.WebApi.Lib.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hawk.Domain.Entities;
    using Hawk.Domain.Entities.Payment;
    using Hawk.Domain.Entities.Transactions;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    internal static class TransactionMapping
    {
        private static readonly IReadOnlyDictionary<string, Func<Option<Guid>, Option<Pay>, Option<Account>, Try<Transaction>>> Types =
            new Dictionary<string, Func<Option<Guid>, Option<Pay>, Option<Account>, Try<Transaction>>>
            {
                { "Debit", Debit.CreateWith },
                { "Credit", Credit.CreateWith },
            };

        public static Paged<Models.Transaction.Get.Transaction> ToModel(this Paged<Transaction> @this)
        {
            var model = @this
                .Data
                .Select(item => (Models.Transaction.Get.Transaction)item)
                .ToList();

            return new Paged<Models.Transaction.Get.Transaction>(model, @this.Skip, @this.Limit);
        }

        public static Transaction ToEntity(this Models.Transaction.Post.Transaction @this) => Map(
            @this.Type,
            Guid.NewGuid(),
            @this.Store,
            @this.Tags,
            @this.Account,
            @this.Payment,
            @this.Parcel);

        public static Transaction ToEntity(this Models.Transaction.Get.Transaction @this) => Map(
            @this.Type,
            new Guid(@this.Id),
            @this.Store,
            @this.Tags,
            @this.Account,
            @this.Payment,
            @this.Parcel);

        private static Transaction Map(
            string type,
            Guid id,
            string store,
            IEnumerable<string> tags,
            Models.Transaction.Account account,
            Models.Transaction.Payment payment,
            Models.Transaction.Parcel parcel)
        {
            if (!Types.TryGetValue(type, out var createWith))
            {
                return null;
            }

            Option<Tag> MapTag(string tag) => Tag.CreateWith(tag);

            var transaction = createWith(id, payment, account).GetOrElse(default);
            transaction.AddTags(tags.Select(MapTag).ToList());
            transaction.UpdateStore(Store.CreateWith(store));
            transaction.SplittedIn(parcel);

            return transaction;
        }
    }
}