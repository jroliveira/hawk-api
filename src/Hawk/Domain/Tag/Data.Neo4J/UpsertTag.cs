namespace Hawk.Domain.Tag.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
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

        public Task<Try<Tag>> Execute(Option<Email> email, Option<Tag> entity) => entity.Match(
            some => this.Execute(email, some.Value, some),
            () => Task(Failure<Tag>(new NullObjectException("Tag is required."))));

        public Task<Try<Tag>> Execute(Option<Email> email, Option<string> name, Option<Tag> entity) =>
            email
            && name
            && entity
                ? this.connection.ExecuteCypherScalar(
                    MapTag,
                    Statement,
                    new
                    {
                        email = email.Get().Value,
                        name = name.Get(),
                        newName = entity.Get().Value,
                    })
                : Task(Failure<Tag>(new NullObjectException("Parameters are required.")));
    }
}
