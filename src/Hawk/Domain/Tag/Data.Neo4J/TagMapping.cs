namespace Hawk.Domain.Tag.Data.Neo4J
{
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;

    internal static class TagMapping
    {
        internal static Try<Tag> MapTag(IRecord data) => MapTag(MapRecord(data, "data"));

        internal static Try<Tag> MapTag(Option<Neo4JRecord> record) => record.Match(
            some => NewTag(some.Get<string>("name"), some.Get<uint>("transactions")),
            () => new NotFoundException("Tag not found."));
    }
}
