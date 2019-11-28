namespace Hawk.Infrastructure.Logging.Converters
{
    using System;
    using System.Net;

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
            var ep = (IPEndPoint)value;
            var jo = new JObject
            {
                { "Address", JToken.FromObject(ep.Address, serializer) },
                { "Port", ep.Port },
            };

            jo.WriteTo(writer);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var address = jo["Address"].ToObject<IPAddress>(serializer);
            var port = (int)jo["Port"];

            return new IPEndPoint(address, port);
        }
    }
}
