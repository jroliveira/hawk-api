namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Neo4j.Driver.V1;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static System.ComponentModel.TypeDescriptor;

    internal sealed class Record
    {
        private readonly IDictionary<string, object> data;

        internal Record()
            : this(null)
        {
        }

        internal Record(object record)
        {
            if (record == null)
            {
                return;
            }

            if (record is IEntity node)
            {
                this.data = node.Properties.ToDictionary(item => item.Key, item => item.Value);
                return;
            }

            this.data = record.As<IDictionary<string, object>>();
        }

        internal Option<Record> GetRecord(string key)
        {
            if (!this.Has(key))
            {
                return None;
            }

            return new Record(this.data[key]);
        }

        internal IEnumerable<string> GetList(string key) => this
            .Get<IList<object>>(key)
            .GetOrElse(new List<object>())
            ?.Select(item => item.ToString());

        internal Option<TValue> Get<TValue>(string key)
        {
            if (!this.Has(key))
            {
                return None;
            }

            switch (typeof(TValue))
            {
                case Type guidType when guidType == typeof(Guid):
                    return (TValue)GetConverter(typeof(Guid)).ConvertFromInvariantString(this.data[key].As<string>());
                default:
                    return this.data[key].As<TValue>();
            }
        }

        internal bool Has(string key) => this.data != null && this.data.ContainsKey(key);
    }
}
