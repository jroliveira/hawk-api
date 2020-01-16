namespace Hawk.Domain.Store.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Store.Data.Neo4J.StoreMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertStore : IUpsertStore
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Store", "Data.Neo4J", "UpsertStore.cql"));
        private readonly Neo4JConnection connection;

        public UpsertStore(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Store>> Execute(Email email, string name, Option<Store> entity) => entity.Match(
            async some =>
            {
                var data = await this.connection.ExecuteCypherScalar(
                    MapStore,
                    Statement,
                    new
                    {
                        email = email.Value,
                        name,
                        newName = some.Value,
                    });

                return data.Match<Try<Store>>(
                    _ => _,
                    store => store.Store);
            },
            () => Task(Failure<Store>(new NullObjectException("Store is required."))));
    }
}
