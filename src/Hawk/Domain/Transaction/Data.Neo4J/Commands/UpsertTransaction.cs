namespace Hawk.Domain.Transaction.Data.Neo4J.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Commands;
    using Hawk.Domain.Transaction;
    using Hawk.Domain.Transaction.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.IO.Path;
    using static System.String;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class UpsertTransaction : Command<UpsertParam<Guid, Transaction>>, IUpsertTransaction
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Transaction", "Data.Neo4J", "Commands", "UpsertTransaction.cql"));
        private readonly Neo4JConnection connection;

        public UpsertTransaction(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(UpsertParam<Guid, Transaction> param) => this.connection.ExecuteCypher(
            Statement
                .GetOrElse(Empty)
                .Replace("#type#", param.Entity.Type.ToString()),
            new
            {
                email = param.Email.Value,
                id = param.Entity.Id.ToString(),
                status = param.Entity.Status.ToString(),
                description = param.Entity.Description.GetOrElse(Empty),
                value = param.Entity.Payment.Price.Value,
                year = param.Entity.Payment.Date.Year,
                month = param.Entity.Payment.Date.Month,
                day = param.Entity.Payment.Date.Day,
                currency = param.Entity.Payment.Price.Currency.Id,
                method = param.Entity.Payment.PaymentMethod.Id,
                payee = param.Entity.Payee.Id,
                category = param.Entity.Category.Id,
                tags = param.Entity.Tags.Select(tag => tag.Id).ToArray(),
            });
    }
}
