namespace Hawk.Infrastructure.Data.Neo4J.Commands.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Store;
    using Hawk.Domain.Entities;

    internal sealed class ExcludeCommand : Connection, IExcludeCommand
    {
        public ExcludeCommand(Database database, GetScript file)
            : base(database, file, "Store.Exclude.cql")
        {
        }

        public async Task Execute(Store entity)
        {
            var parameters = new
            {
                name = entity.Name
            };

            await this.Database.Execute(this.Statement, parameters).ConfigureAwait(false);
        }
    }
}