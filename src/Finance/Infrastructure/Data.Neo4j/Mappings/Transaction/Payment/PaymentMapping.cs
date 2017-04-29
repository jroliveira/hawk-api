namespace Finance.Infrastructure.Data.Neo4j.Mappings.Transaction.Payment
{
    using System;
    using System.Globalization;

    using Finance.Entities.Transaction.Payment;

    public class PaymentMapping
    {
        private readonly MethodMapping methodMapping;

        public PaymentMapping(MethodMapping methodMapping)
        {
            this.methodMapping = methodMapping;
        }

        public Payment MapFrom(Record record)
        {
            var method = this.methodMapping.MapFrom(record);
            var date = record.Get("date");

            return new Payment(
                record.Get<double>("value"),
                DateTime.ParseExact(date, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture))
            {
                Method = method
            };
        }
    }
}