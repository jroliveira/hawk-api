namespace Hawk.Infrastructure.Data.Neo4J.Queries.Account
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Queries.Account;
    using Hawk.Infrastructure.Data.Neo4J.Mappings;

    internal sealed class GetByEmailQuery : QueryBase, IGetByEmailQuery
    {
        private readonly AccountMapping mapping;

        public GetByEmailQuery(Database database, AccountMapping mapping, GetScript file)
            : base(database, file)
        {
            this.mapping = mapping;
        }

        public async Task<Account> GetResult(string email)
        {
            var query = this.File.ReadAllText("Account.GetByEmail.cql");
            var parameters = new
            {
                email
            };

            var entities = await this.Database.Execute(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return entities.FirstOrDefault();
        }
    }
}