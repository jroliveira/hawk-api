﻿namespace Hawk.Infrastructure.Logging
{
    using System;
    using System.Runtime.Serialization;

    using static Clock;

    using static JsonSettings;

    using static Newtonsoft.Json.JsonConvert;

    public sealed class DefaultLogData : ILogData
    {
        public DefaultLogData(LogLevel level, string tracking, object data)
        {
            this.Level = level;
            this.Tracking = tracking;
            this.Data = data;
        }

        public DateTime DateTime => UtcNow();

        public LogLevel Level { get; }

        public string Tracking { get; }

        public object Data { get; }

        public override string ToString()
        {
            try
            {
                return SerializeObject(this, JsonSerializerSettings);
            }
            catch (Exception exception)
            {
                throw new SerializationException("Cannot serialize object defaultLogData.", exception);
            }
        }
    }
}
