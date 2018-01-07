namespace Hawk.Infrastructure.Data.Neo4J.Mappings.Payment
{
    using System;

    using Hawk.Domain.Entities.Payment;

    internal sealed class PayMapping
    {
        private readonly PriceMapping priceMapping;
        private readonly MethodMapping methodMapping;

        public PayMapping(PriceMapping priceMapping, MethodMapping methodMapping)
        {
            this.priceMapping = priceMapping;
            this.methodMapping = methodMapping;
        }

        public Pay MapFrom(Record record)
        {
            var price = this.priceMapping.MapFrom(record);
            var method = this.methodMapping.MapFrom(record.GetRecord("method"));
            var date = new DateTime(
                record.Get<int>("year"),
                record.Get<int>("month"),
                record.Get<int>("day"));

            return new Pay(
                price,
                date,
                method);
        }
    }
}