namespace Hawk.Domain.Category.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Category.Data.Neo4J.CategoryMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertCategory : IUpsertCategory
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Category", "Data.Neo4J", "UpsertCategory.cql"));
        private readonly Neo4JConnection connection;

        public UpsertCategory(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Category>> Execute(Email email, string name, Option<Category> entity) => entity.Match(
            some => this.connection.ExecuteCypherScalar(
                MapCategory,
                Statement,
                new
                {
                    email = email.Value,
                    name,
                    newName = some.Value,
                }),
            () => Task(Failure<Category>(new NullObjectException("Category is required."))));
    }
}
