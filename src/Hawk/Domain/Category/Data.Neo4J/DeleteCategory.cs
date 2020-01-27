namespace Hawk.Domain.Category.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteCategory : IDeleteCategory
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Category", "Data.Neo4J", "DeleteCategory.cql"));
        private readonly Neo4JConnection connection;

        public DeleteCategory(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Unit>> Execute(Email email, string name) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = email.Value,
                name,
            });
    }
}
