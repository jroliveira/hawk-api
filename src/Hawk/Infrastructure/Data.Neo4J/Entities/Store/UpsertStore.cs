namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Store.StoreMapping;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertStore : IUpsertStore
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Store", "UpsertStore.cql"));
        private readonly Neo4JConnection connection;

        public UpsertStore(Neo4JConnection connection) => this.connection = connection;

        public async Task<Try<Store>> Execute(Email email, string name, Option<Store> entity)
        {
            if (!entity.IsDefined)
            {
                return Failure<Store>(new NullReferenceException("Store is required."));
            }

            var data = await this.connection.ExecuteCypherScalar(
                MapStore,
                Statement,
                new
                {
                    email = email.ToString(),
                    name,
                    newName = entity.Get().Name,
                });

            return data.Match<Try<Store>>(
                _ => _,
                store => store.Store);
        }
    }
}
