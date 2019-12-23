namespace Hawk.Infrastructure.Data.Neo4J.Entities.Tag
{
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Tag.Tag;

    using static Neo4JRecord;

    internal static class TagMapping
    {
        private const string Name = "name";
        private const string Total = "total";

        internal static Try<(Tag Tag, uint Count)> MapTag(IRecord data) => MapTag(MapRecord(data, "data"));

        internal static Try<(Tag Tag, uint Count)> MapTag(Option<Neo4JRecord> record) => record.Match(
            some =>
            {
                var total = some.Get<uint>(Total);
                if (!total.IsDefined)
                {
                    return new InvalidObjectException("Invalid tag.");
                }

                return NewTag(some.Get<string>(Name)).Match<Try<(Tag, uint)>>(
                    _ => _,
                    tag => (tag, total.Get()));
            },
            () => new NotFoundException("Tag not found."));
    }
}
