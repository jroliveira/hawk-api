namespace Hawk.Infrastructure.Data.Neo4J.Entities.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Tag.TagMapping;

    internal sealed class GetTagByName : IGetTagByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Tag", "GetTagByName.cql"));
        private readonly Neo4JConnection connection;

        public GetTagByName(Neo4JConnection connection) => this.connection = connection;

        public async Task<Try<Tag>> GetResult(Email email, string name)
        {
            var data = await this.connection.ExecuteCypherScalar(
                MapTag,
                Statement,
                new
                {
                    email = email.ToString(),
                    name,
                });

            return data.Match<Try<Tag>>(
                _ => _,
                tag => tag.Tag);
        }
    }
}
