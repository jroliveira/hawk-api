namespace Finance.Infrastructure.Data.Neo4j.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Neo4j.Driver.V1;

    public static class RecordExtension
    {
        public static Record GetRecord(this IRecord record)
        {
            if (record == null)
            {
                return null;
            }

            return new Record(record.As<IDictionary<string, object>>());
        }

        public static Record GetRecord(this IRecord record, string key)
        {
            if (record == null || !record.Keys.Contains(key))
            {
                return null;
            }

            return new Record(record[key]);
        }
    }
}
