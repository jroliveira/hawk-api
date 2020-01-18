namespace Hawk.Domain.Tag.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Tag.Data.Neo4J.TagMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetTagByName : IGetTagByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Tag", "Data.Neo4J", "GetTagByName.cql"));
        private readonly Neo4JConnection connection;

        public GetTagByName(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Tag>> GetResult(Email email, string name) => this.connection.ExecuteCypherScalar(
            MapTag,
            Statement,
            new
            {
                email = email.Value,
                name,
            });
    }
}
