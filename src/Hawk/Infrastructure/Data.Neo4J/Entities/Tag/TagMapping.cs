namespace Hawk.Infrastructure.Data.Neo4J.Entities.Tag
{
    using System;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Tag.Tag;

    internal static class TagMapping
    {
        private const string Name = "name";
        private const string Total = "total";

        internal static Try<(Tag Tag, uint Count)> MapFrom(IRecord data) => MapFrom(data.GetRecord("data"));

        internal static Try<(Tag Tag, uint Count)> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<string>(Name)).Match<Try<(Tag, uint)>>(
                _ => _,
                paymentMethod => (paymentMethod, record.Get<uint>(Total).GetOrElse(0u))),
            () => new NullReferenceException("Tag cannot be null."));
    }
}
