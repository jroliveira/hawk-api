namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteStore : IDeleteStore
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Store", "DeleteStore.cql"));
        private readonly Neo4JConnection connection;

        public DeleteStore(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Unit>> Execute(Email email, string name) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = email.ToString(),
                name,
            });
    }
}
