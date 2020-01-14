namespace Hawk.Infrastructure.Data.Neo4J.Entities.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class DeleteTag : IDeleteTag
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Tag", "DeleteTag.cql"));
        private readonly Neo4JConnection connection;

        public DeleteTag(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Unit>> Execute(Email email, string name) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = email.ToString(),
                name,
            });
    }
}
