namespace Hawk.Domain.Category.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category;
    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Category.Data.Neo4J.CategoryMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetCategoryByName : Query<GetByIdParam<string>, Category>, IGetCategoryByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Category", "Data.Neo4J", "Queries", "GetCategoryByName.cql"));
        private readonly Neo4JConnection connection;

        public GetCategoryByName(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Category>> GetResult(GetByIdParam<string> param) => this.connection.ExecuteCypherScalar(
            MapCategory,
            Statement,
            new
            {
                email = param.Email.Value,
                name = param.Id,
            });
    }
}
