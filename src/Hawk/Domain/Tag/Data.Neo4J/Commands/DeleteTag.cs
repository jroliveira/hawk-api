namespace Hawk.Domain.Tag.Data.Neo4J.Commands
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Commands;
    using Hawk.Domain.Tag.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteTag : Command<DeleteParam<string>>, IDeleteTag
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Tag", "Data.Neo4J", "Commands", "DeleteTag.cql"));
        private readonly Neo4JConnection connection;

        public DeleteTag(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(DeleteParam<string> param) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = param.Email.Value,
                name = param.Id,
            });
    }
}
