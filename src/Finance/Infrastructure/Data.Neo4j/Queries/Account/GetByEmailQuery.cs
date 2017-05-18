namespace Finance.Infrastructure.Data.Neo4j.Queries.Account
{
    using System.Linq;

    using Finance.Entities;
    using Finance.Infrastructure.Data.Neo4j.Mappings;

    public class GetByEmailQuery : QueryBase<Account>
    {
        public GetByEmailQuery(Database database, IMapping<Account> mapping, File file)
            : base(database, mapping, file)
        {
        }

        public virtual Account GetResult(string email)
        {
            var query = this.File.ReadAllText(@"Account\GetByEmail.cql");
            var parameters = new
            {
                email
            };

            var entities = this.Database.Execute(this.Mapping.MapFrom, query, parameters);

            return entities.FirstOrDefault();
        }
    }
}