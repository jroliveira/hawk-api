﻿namespace Hawk.Infrastructure.Data.Neo4j.Mappings.Payment
{
    using Hawk.Entities.Transaction.Payment;
    using Hawk.Infrastructure.Data.Neo4j.Extensions;

    using global::Neo4j.Driver.V1;

    public class MethodMapping
    {
        public Method MapFrom(IRecord data)
        {
            return this.MapFrom(data.GetRecord("data"));
        }

        public Method MapFrom(Record record)
        {
            if (record == null || !record.Any())
            {
                return null;
            }

            var name = record.Get("name");
            return new Method(name)
            {
                Total = record.Get<int>("total")
            };
        }
    }
}