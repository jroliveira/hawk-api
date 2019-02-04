namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Store;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J.Entities.Store;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver.V1;

    internal static class TransactionMapping
    {
        private const string Type = "type";
        private const string Payment = "payment";
        private const string Id = "id";
        private const string Tags = "tags";
        private const string Store = "store";
        private const string Data = "data";

        private static readonly IReadOnlyDictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IReadOnlyCollection<Tag>>, Try<Transaction>>> Types =
            new Dictionary<string, Func<Option<Guid>, Option<Payment>, Option<Store>, Option<IReadOnlyCollection<Tag>>, Try<Transaction>>>
        {
            { "Debit", Debit.CreateWith },
            { "Credit", Credit.CreateWith },
        };

        internal static Try<Transaction> MapFrom(IRecord data) => data.GetRecord(Data).Match(
            record =>
            {
                var type = record
                    .GetList(Type)
                    .Single(t => Types.ContainsKey(t.ToString()));

                if (!Types.TryGetValue(type, out var createWith))
                {
                    return new InvalidObjectException("Invalid transaction.");
                }

                var tags = record
                    .GetList(Tags)
                    .Select(tag => Tag.CreateWith(tag))
                    .ToList();

                if (tags.Any(tag => tag.IsFailure))
                {
                    return new InvalidObjectException("Invalid transaction.");
                }

                return createWith(
                    record.Get<Guid>(Id),
                    PaymentMapping.MapFrom(record.GetRecord(Payment)),
                    StoreMapping.MapFrom(record.GetRecord(Store)),
                    new List<Tag>(tags.Select(tag => tag.Get())));
            },
            () => new NotFoundException("Transaction not found."));
    }
}
