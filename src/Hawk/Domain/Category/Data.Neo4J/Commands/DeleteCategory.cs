namespace Hawk.Domain.Category.Data.Neo4J.Commands
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteCategory : Command<DeleteParam<string>>, IDeleteCategory
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Category", "Data.Neo4J", "Commands", "DeleteCategory.cql"));
        private readonly Neo4JConnection connection;

        public DeleteCategory(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(DeleteParam<string> param) => this.connection.ExecuteCypher(
            StatementOption,
            new
            {
                email = param.Email.Value,
                name = param.Id,
            });
    }
}
