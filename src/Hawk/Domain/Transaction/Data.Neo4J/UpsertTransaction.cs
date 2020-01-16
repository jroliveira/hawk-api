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

        public Task<Try<Transaction>> Execute(Email email, Option<Transaction> entity) => entity.Match(
            some => this.connection.ExecuteCypherScalar(
                MapTransaction,
                Statement
                    .GetOrElse(Empty)
                    .Replace("#type#", some.Type),
                new
                {
                    email = email.Value,
                    id = some.Id.ToString(),
                    value = some.Payment.Price.Value,
                    year = some.Payment.Date.Year,
                    month = some.Payment.Date.Month,
                    day = some.Payment.Date.Day,
                    currency = some.Payment.Price.Currency.Value,
                    method = some.Payment.PaymentMethod.Value,
                    store = some.Store.Value,
                    tags = some.Tags.Select(tag => tag.Value).ToArray(),
                }),
            () => Task(Failure<Transaction>(new NullObjectException("Transaction is required."))));
    }
}
