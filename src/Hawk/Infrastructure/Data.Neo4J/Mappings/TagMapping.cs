namespace Hawk.Infrastructure.Data.Neo4J.Mappings
{
    using System;
    using Hawk.Domain.Entities;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Neo4j.Driver.V1;

    internal static class TagMapping
    {
        private const string Name = "name";
        private const string Total = "total";

        public static Try<(Tag Tag, uint Count)> MapFrom(IRecord data) => MapFrom(data.GetRecord("data"));

        public static Try<(Tag Tag, uint Count)> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => Tag.CreateWith(record.Get<string>(Name)).Match<Try<(Tag, uint)>>(
                _ => _,
                method => (method, record.Get<uint>(Total).GetOrElse(0u))),
            () => new NullReferenceException("Tag cannot be null."));
    }
}