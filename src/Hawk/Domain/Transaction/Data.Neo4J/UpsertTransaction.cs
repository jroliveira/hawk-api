namespace Hawk.Domain.Transaction.Data.Neo4J
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.IO.Path;
    using static System.String;

    using static Hawk.Domain.Transaction.Data.Neo4J.TransactionMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertTransaction : IUpsertTransaction
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Transaction", "Data.Neo4J", "UpsertTransaction.cql"));
        private readonly Neo4JConnection connection;

        public UpsertTransaction(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Transaction>> Execute(Option<Email> email, Option<Transaction> entity) =>
            email
            && entity
                ? this.connection.ExecuteCypherScalar(
                    MapTransaction,
                    Statement
                        .GetOrElse(Empty)
                        .Replace("#type#", entity.Get().Type.ToString()),
                    new
                    {
                        email = email.Get().Value,
                        id = entity.Get().Id.ToString(),
                        status = entity.Get().Status.ToString(),
                        description = entity.Get().Description.GetOrElse(Empty),
                        value = entity.Get().Payment.Price.Value,
                        year = entity.Get().Payment.Date.Year,
                        month = entity.Get().Payment.Date.Month,
                        day = entity.Get().Payment.Date.Day,
                        currency = entity.Get().Payment.Price.Currency.Value,
                        method = entity.Get().Payment.PaymentMethod.Value,
                        payee = entity.Get().Payee.Value,
                        category = entity.Get().Category.Value,
                        tags = entity.Get().Tags.Select(tag => tag.Value).ToArray(),
                    })
                : Task(Failure<Transaction>(new NullObjectException("Parameters are required.")));
    }
}
