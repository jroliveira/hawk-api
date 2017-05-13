namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction.Payment
{
    using System;
    using System.Globalization;

    using Finance.Entities.Transaction.Payment;

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
            var date = record.Get("date");

            return new Payment(
                record.Get<double>("value"),
                DateTime.ParseExact(date, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture),
                currency)
            {
                Method = method
            };
        }
    }
}