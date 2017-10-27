namespace Hawk.Infrastructure.Data.Neo4j.Mappings.Payment
{
    using System;

    using Hawk.Entities.Transaction.Payment;

    public class PaymentMapping
    {
        private readonly CurrencyMapping currencyMapping;
        private readonly MethodMapping methodMapping;

        public PaymentMapping(CurrencyMapping currencyMapping, MethodMapping methodMapping)
        {
            this.currencyMapping = currencyMapping;
            this.methodMapping = methodMapping;
        }

        public Payment MapFrom(Record record)
        {
            var currency = this.currencyMapping.MapFrom(record.GetRecord("currency"));
            var method = this.methodMapping.MapFrom(record.GetRecord("method"));
            var year = record.Get<int>("year");
            var month = record.Get<int>("month");
            var day = record.Get<int>("day");

            return new Payment(
                record.Get<double>("value"),
                new DateTime(year, month, day),
                currency)
            {
                Method = method
            };
        }
    }
}