namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver;

    using static System.ComponentModel.TypeDescriptor;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;
    using static Hawk.Infrastructure.Logging.Logger;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class Neo4JRecord
    {
        private readonly IDictionary<string, object> data;

        internal Neo4JRecord(object record)
        {
            switch (record)
            {
                case null:
                    this.data = new Dictionary<string, object>();
                    return;
                case IEntity node:
                    this.data = node.Properties.ToDictionary(item => item.Key, item => item.Value);
                    return;
                default:
                    this.data = record.As<IDictionary<string, object>>();
                    break;
            }
        }

        internal static Option<Neo4JRecord> MapRecord(IRecord record, string key)
        {
            if (record == default || !record.Keys.Contains(key))
            {
                return None();
            }

            return new Neo4JRecord(record[key]);
        }

        internal Option<Neo4JRecord> GetRecord(string key)
        {
            if (!this.Has(key))
            {
                return None();
            }

            return new Neo4JRecord(this.data[key]);
        }

        internal IEnumerable<string> GetList(string key) => this
            .Get<IList<object>>(key)
            .GetOrElse(new List<object>())
            .Select(item => item.ToString());

        internal Option<TValue> Get<TValue>(string key)
        {
            if (!this.Has(key))
            {
                return None();
            }

            try
            {
                return typeof(TValue) switch
                {
                    { } guidType when guidType == typeof(Guid) => (TValue)GetConverter(typeof(Guid)).ConvertFromInvariantString(this.data[key].As<string>()),
                    _ => this.data[key].As<TValue>()
                };
            }
            catch (Exception exception)
            {
                LogError("Get key value threw an exception.", new { Key = key }, HandleException(exception));
                return None();
            }
        }

        private bool Has(string key) => this.data != default && this.data.ContainsKey(key);
    }
}
