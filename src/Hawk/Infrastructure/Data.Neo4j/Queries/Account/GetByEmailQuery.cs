namespace Hawk.Infrastructure.Data.Neo4j.Queries.Account
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Entities;
    using Hawk.Infrastructure.Data.Neo4j.Mappings;

    public class GetByEmailQuery : QueryBase
    {
        private readonly AccountMapping mapping;

        public GetByEmailQuery(Database database, AccountMapping mapping, GetScript file)
            : base(database, file)
        {
            this.mapping = mapping;
        }

        public virtual async Task<Account> GetResultAsync(string email)
        {
            var query = this.File.ReadAllText("Account.GetByEmail.cql");
            var parameters = new
            {
                email
            };

            var entities = await this.Database.ExecuteAsync(this.mapping.MapFrom, query, parameters).ConfigureAwait(false);

            return entities.FirstOrDefault();
        }
    }
}