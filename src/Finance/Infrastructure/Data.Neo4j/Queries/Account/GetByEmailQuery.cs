namespace Finance.Infrastructure.Data.Neo4j.Queries.Account
{
    using System.Linq;

    using Finance.Entities;
    using Finance.Infrastructure.Data.Neo4j.Mappings;

    public class GetByEmailQuery : QueryBase
    {
        private readonly AccountMapping mapping;

        public GetByEmailQuery(Database database, AccountMapping mapping, GetScript file)
            : base(database, file)
        {
            this.mapping = mapping;
        }

        public virtual Account GetResult(string email)
        {
            var query = this.File.ReadAllText("Account.GetByEmail.cql");
            var parameters = new
            {
                email
            };

            var entities = this.Database.Execute(this.mapping.MapFrom, query, parameters);

            return entities.FirstOrDefault();
        }
    }
}