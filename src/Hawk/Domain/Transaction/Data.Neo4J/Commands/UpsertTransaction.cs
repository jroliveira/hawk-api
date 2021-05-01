namespace Hawk.Domain.Transaction.Data.Neo4J.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Installment;
    using Hawk.Domain.Shared;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Domain.Transaction;
    using Hawk.Domain.Transaction.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.Convert;
    using static System.IO.Path;
    using static System.String;
    using static System.Threading.Tasks.Task;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class UpsertTransaction : Command<UpsertParam<Guid, Transaction>>, IUpsertTransaction
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Transaction", "Data.Neo4J", "Commands", "UpsertTransaction.cql"));
        private readonly Neo4JConnection connection;

        public UpsertTransaction(Neo4JConnection connection) => this.connection = connection;

        protected override async Task<Try<Unit>> Execute(UpsertParam<Guid, Transaction> param)
        {
            await WhenAll(new List<Task<Try<Unit>>>(param.Entity.InstallmentOption
                .Fold(new List<Task<Try<Unit>>>())(installment => installment.GenerateTransactions(param.Entity)
                    .Fold(new List<Task<Try<Unit>>>())(transactions => new List<Task<Try<Unit>>>(transactions
                        .Where(_ => _)
                        .Select(transactionOption => transactionOption.Get())
                        .Select(transaction => this.Execute(param.Email, transaction.Id, transaction))))))
            {
                this.Execute(param.Email, param.Id, param.Entity),
            });

            return new Try<Unit>(new Unit());
        }

        private Task<Try<Unit>> Execute(Email email, Guid id, Transaction entity) => this.connection.ExecuteCypher(
            StatementOption
                .GetOrElse(Empty)
                .Replace("#type#", entity.Type.ToString()),
            new
            {
                email = email.Value,
                id = id.ToString(),
                status = entity.Status.ToString(),
                description = entity.DescriptionOption.GetOrElse(Empty),
                value = entity.Payment.Cost.Value,
                year = entity.Payment.Date.Year,
                month = entity.Payment.Date.Month,
                day = entity.Payment.Date.Day,
                currency = entity.Payment.Cost.Currency.Id,
                method = entity.Payment.PaymentMethod.Id,
                payee = entity.Payee.Id,
                category = entity.Category.Id,
                tags = entity.Tags.Select(tag => tag.Id).ToArray(),
                installment = entity.InstallmentOption.Fold<Installment, dynamic?>(null)(installment => new
                {
                    id = installment.Id.ToString(),
                    total = ToInt32(installment.Installments.Total),
                    frequency = installment.Frequency.ToString(),
                }),
            });
    }
}
