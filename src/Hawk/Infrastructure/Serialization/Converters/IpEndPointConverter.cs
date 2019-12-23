namespace Hawk.Infrastructure.Serialization.Converters
{
    using System;
    using System.Net;

    using Neo4j.Driver;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal sealed class IpEndPointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(IPEndPoint);

        public override void WriteJson(
            JsonWriter writer,
            object? value,
            JsonSerializer serializer)
        {
            if (value == default)
            {
                return;
            }

            var ep = (IPEndPoint)value;
            var jo = new JObject
            {
                { "Address", JToken.FromObject(ep.Address, serializer) },
                { "Port", ep.Port },
            };

            jo.WriteTo(writer);
        }

        public override object? ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var address = jo.GetValue("Address")?.ToObject<IPAddress>(serializer);
            if (address == null)
            {
                return default;
            }

            var port = jo.GetValue("Port").As<int>();

            return new IPEndPoint(address, port);
        }
    }
}
