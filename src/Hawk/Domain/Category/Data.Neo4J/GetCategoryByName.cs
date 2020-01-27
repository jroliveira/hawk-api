namespace Hawk.Domain.Category.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Category.Data.Neo4J.CategoryMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetCategoryByName : IGetCategoryByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Category", "Data.Neo4J", "GetCategoryByName.cql"));
        private readonly Neo4JConnection connection;

        public GetCategoryByName(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Category>> GetResult(Email email, string name) => this.connection.ExecuteCypherScalar(
            MapCategory,
            Statement,
            new
            {
                email = email.Value,
                name,
            });
    }
}
