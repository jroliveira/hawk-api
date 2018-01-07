namespace Hawk.Infrastructure.Data.Neo4J.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Neo4j.Driver.V1;

    internal static class RecordExtension
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
