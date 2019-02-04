﻿namespace Hawk.Infrastructure.Data.Neo4J.Entities.Transaction
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Transaction.TransactionMapping;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertTransaction : IUpsertTransaction
    {
        private static readonly Option<string> Statement = ReadAll("Transaction.UpsertTransaction.cql");
        private readonly Database database;

        public UpsertTransaction(Database database) => this.database = database;

        public Task<Try<Transaction>> Execute(Email email, Option<Transaction> entity)
        {
            if (!entity.IsDefined)
            {
                return Task(Failure<Transaction>(new NullReferenceException("Transaction is required.")));
            }

            var statement = Statement.GetOrElse(Empty).Replace("#type#", entity.Get().GetType().Name);

            var parameters = new
            {
                email = email.ToString(),
                id = entity.Get().Id.ToString(),
                value = entity.Get().Payment.Price.Value,
                year = entity.Get().Payment.Date.Year,
                month = entity.Get().Payment.Date.Month,
                day = entity.Get().Payment.Date.Day,
                currency = entity.Get().Payment.Price.Currency.Name,
                method = entity.Get().Payment.PaymentMethod.Name,
                store = entity.Get().Store.Name,
                tags = entity.Get().Tags.Select(tag => tag.Name).ToArray(),
            };

            return this.database.ExecuteScalar(MapFrom, statement, parameters);
        }
    }
}
