namespace Hawk.Infrastructure.Data.Neo4J.Commands.Store
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Store;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;
    using static Hawk.Infrastructure.Data.Neo4J.Mappings.StoreMapping;

    internal sealed class CreateCommand : ICreateCommand
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Store.Create.cql");
        private readonly Database database;

        public CreateCommand(Database database) => this.database = database;

        public async Task<Try<Store>> Execute(Store entity)
        {
            var parameters = new
            {
                name = entity.Name,
            };

            var data = await this.database.ExecuteScalar(MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.SelectMany(store => store.Store);
        }
    }
}
