namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using System;

    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure.Data.Neo4J.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver.V1;

    using static Hawk.Domain.Entities.Payment.Method;

    internal static class MethodMapping
    {
        private const string Name = "name";
        private const string Total = "total";
        private const string Data = "data";

        internal static Try<(Method Method, uint Count)> MapFrom(IRecord data) => MapFrom(data.GetRecord(Data));

        internal static Try<(Method Method, uint Count)> MapFrom(Option<Record> recordOption) => recordOption.Match(
            record => CreateWith(record.Get<string>(Name)).Match<Try<(Method, uint)>>(
                _ => _,
                method => (method, record.Get<uint>(Total).GetOrElse(0u))),
            () => new NullReferenceException("Payment method cannot be null."));
    }
}
