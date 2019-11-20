namespace Hawk.Infrastructure.Logging.Converters
{
    using System;
    using System.Net;

    using Newtonsoft.Json;

    using static System.Net.IPAddress;

    internal sealed class IpAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(IPAddress);

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer) => writer.WriteValue(value.ToString());

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer) => Parse((string)reader.Value);
    }
}
