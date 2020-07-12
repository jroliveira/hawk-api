namespace Hawk.Domain.Tag.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver;

    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Data.Neo4J.Neo4JRecord;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class TagMapping
    {
        internal static Try<Tag> MapTag(in IRecord data) => MapTag(MapRecord(data, "data"));

        internal static Try<Tag> MapTag(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Tag>(NotFound(nameof(Tag))))(record => NewTag(
                record.Get<string>("name"),
                record.Get<uint>("transactions")));
    }
}
