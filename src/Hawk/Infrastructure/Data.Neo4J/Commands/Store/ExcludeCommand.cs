namespace Hawk.Infrastructure.Data.Neo4J.Commands.Store
{
    using System.Threading.Tasks;
    using Hawk.Domain.Commands.Store;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using static System.String;

    internal sealed class ExcludeCommand : IExcludeCommand
    {
        private static readonly Option<string> Statement = CypherScript.ReadAll("Store.Exclude.cql");
        private readonly Database database;

        public ExcludeCommand(Database database) => this.database = database;

        public Task<Try<Unit>> Execute(Store entity)
        {
            var parameters = new
            {
                name = entity.Name,
            };

            return this.database.Execute(Statement.GetOrElse(Empty), parameters);
        }
    }
}