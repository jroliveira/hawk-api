namespace Hawk.Infrastructure.Data.Neo4J.Queries.Account
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Account;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class GetByEmailQuery : Connection, IGetByEmailQuery
    {
        private readonly AccountMapping mapping;

        public GetByEmailQuery(Database database, GetScript file, AccountMapping mapping)
            : base(database, file, "Account.GetByEmail.cql")
        {
            Guard.NotNull(mapping, nameof(mapping), "Account mapping cannot be null.");

            this.mapping = mapping;
        }

        public async Task<Account> GetResult(string email)
        {
            var parameters = new
            {
                email
            };

            var entities = await this.Database.Execute(this.mapping.MapFrom, this.Statement, parameters).ConfigureAwait(false);

            return entities.FirstOrDefault();
        }
    }
}