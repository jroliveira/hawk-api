namespace Hawk.Infrastructure.Data.Neo4J.Entities.Tag
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.Tag.TagMapping;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class UpsertTag : IUpsertTag
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Tag", "UpsertTag.cql"));
        private readonly Neo4JConnection connection;

        public UpsertTag(Neo4JConnection connection) => this.connection = connection;

        public async Task<Try<Tag>> Execute(Email email, string name, Option<Tag> entity)
        {
            if (!entity.IsDefined)
            {
                return Failure<Tag>(new NullReferenceException("Tag is required."));
            }

            var data = await this.connection.ExecuteCypherScalar(
                MapTag,
                Statement,
                new
                {
                    email = email.ToString(),
                    name,
                    newName = entity.Get().Name,
                });

            return data.Match<Try<Tag>>(
                _ => _,
                tag => tag.Tag);
        }
    }
}
