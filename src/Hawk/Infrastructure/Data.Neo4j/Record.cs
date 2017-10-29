namespace Hawk.Infrastructure.Data.Neo4j
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Neo4j.Driver.V1;

    public class Record
    {
        private readonly IDictionary<string, object> data;

        public Record(object record)
        {
            if (record is IEntity node)
            {
                this.data = node.Properties.ToDictionary(item => item.Key, item => item.Value);
                return;
            }

            this.data = record.As<IDictionary<string, object>>();
        }

        public Record GetRecord(string key)
        {
            if (this.data == null || !this.data.ContainsKey(key))
            {
                return null;
            }

            return new Record(this.data[key]);
        }

        public IEnumerable<string> GetList(string key)
        {
            return this
                .Get<IList<object>>(key)
                ?.Select(item => item.ToString());
        }

        public string Get(string key)
        {
            return this.Get<string>(key);
        }

        public TValue Get<TValue>(string key)
        {
            var value = default(TValue);

            if (this.data == null || !this.data.ContainsKey(key))
            {
                return value;
            }

            return this.data[key].As<TValue>();
        }

        public bool Has(string key)
        {
            return this.data.ContainsKey(key);
        }

        public bool Any()
        {
            return this.data != null;
        }

        public Guid GetGuid()
        {
            var id = this.Get("id");

            return new Guid(id);
        }
    }
}