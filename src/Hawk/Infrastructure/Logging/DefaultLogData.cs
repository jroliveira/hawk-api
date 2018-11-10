namespace Hawk.Infrastructure.Logging
{
    using System;
    using System.Runtime.Serialization;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public sealed class DefaultLogData : ILogData
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        public DefaultLogData(LogLevel level, string tracking, object data)
        {
            this.Level = level;
            this.Tracking = tracking;
            this.Data = data;
        }

        public DateTime DateTime => DateTime.UtcNow;

        public LogLevel Level { get; }

        public string Tracking { get; }

        public object Data { get; }

        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(this, Formatting.None, SerializerSettings);
            }
            catch (Exception exception)
            {
                throw new SerializationException("Cannot serialize object defaultLogData.", exception);
            }
        }
    }
}
