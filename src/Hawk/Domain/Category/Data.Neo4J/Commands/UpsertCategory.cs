namespace Hawk.Domain.Category.Data.Neo4J.Commands
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category;
    using Hawk.Domain.Category.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class UpsertCategory : Command<UpsertParam<string, Category>>, IUpsertCategory
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Category", "Data.Neo4J", "Commands", "UpsertCategory.cql"));
        private readonly Neo4JConnection connection;

        public UpsertCategory(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(UpsertParam<string, Category> param) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = param.Email.Value,
                name = param.Id,
                newName = param.Entity.Id,
            });
    }
}
