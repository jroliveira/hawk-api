namespace Hawk.Domain.Payee.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Payee.Data.Neo4J.PayeeMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertPayee : IUpsertPayee
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Payee", "Data.Neo4J", "UpsertPayee.cql"));
        private readonly Neo4JConnection connection;

        public UpsertPayee(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Payee>> Execute(Email email, string name, Option<Payee> entity) => entity.Match(
            some => this.connection.ExecuteCypherScalar(
                MapPayee,
                Statement,
                new
                {
                    email = email.Value,
                    name,
                    newName = some.Value,
                }),
            () => Task(Failure<Payee>(new NullObjectException("Payee is required."))));
    }
}
