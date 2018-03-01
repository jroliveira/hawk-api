namespace Hawk.Infrastructure.Data.Neo4J.Commands.Store
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Commands.Store;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class CreateCommand : Connection, ICreateCommand
    {
        private readonly StoreMapping mapping;

        public CreateCommand(Database database, GetScript file, StoreMapping mapping)
            : base(database, file, "Store.Create.cql")
        {
            Guard.NotNull(mapping, nameof(mapping), "Transaction mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Store> Execute(Store newEntity, Store entity)
        {
            var parameters = new
            {
                name = entity.Name
            };

            var inserted = await this.Database.Execute(this.mapping.MapFrom, this.Statement, parameters).ConfigureAwait(false);

            return inserted
                .FirstOrDefault()
                .Store;
        }
    }
}