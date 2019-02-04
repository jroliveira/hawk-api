namespace Hawk.Infrastructure.Data.Neo4J.Entities.Tag
{
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Tag.Tag;

    internal static class TagMapping
    {
        private const string Name = "name";
        private const string Total = "total";
        private const string Data = "data";

        internal static Try<(Tag Tag, uint Count)> MapFrom(IRecord data) => MapFrom(data.GetRecord(Data));

        internal static Try<(Tag Tag, uint Count)> MapFrom(Option<Record> record) => record.Match(
            some =>
            {
                var total = some.Get<uint>(Total);
                if (!total.IsDefined)
                {
                    return new InvalidObjectException("Invalid tag.");
                }

                return CreateWith(some.Get<string>(Name)).Match<Try<(Tag, uint)>>(
                    _ => _,
                    tag => (tag, total.Get()));
            },
            () => new NotFoundException("Tag not found."));
    }
}
