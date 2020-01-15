namespace Hawk.Domain.Tag.Data.Neo4J
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Tag.Data.Neo4J.TagMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertTag : IUpsertTag
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Tag", "Data.Neo4J", "UpsertTag.cql"));
        private readonly Neo4JConnection connection;

        public UpsertTag(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Tag>> Execute(Email email, string name, Option<Tag> entity) => entity.Match(
            async some =>
            {
                var data = await this.connection.ExecuteCypherScalar(
                    MapTag,
                    Statement,
                    new
                    {
                        email = email.Value,
                        name,
                        newName = some.Value,
                    });

                return data.Match<Try<Tag>>(
                    _ => _,
                    tag => tag.Tag);
            },
            () => Task(Failure<Tag>(new NullReferenceException("Tag is required."))));
    }
}
