namespace Hawk.WebApi.Infrastructure.Hal.Link
{
    using System;

    using Newtonsoft.Json;

    internal sealed class LinksJsonConverter : JsonConverter<Links>
    {
        private const string Href = "href";
        private const string Rel = "rel";
        private const string Method = "method";

        public override void WriteJson(
            JsonWriter writer,
            Links links,
            JsonSerializer serializer)
        {
            writer.WriteStartObject();

            foreach (var link in links)
            {
                writer.WritePropertyName(link.Rel);
                writer.WriteStartObject();

                writer.WritePropertyName(Href);
                writer.WriteValue(link.Href);

                writer.WritePropertyName(Rel);
                writer.WriteValue(link.Rel);

                writer.WritePropertyName(Method);
                serializer.Serialize(writer, link.Method);

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        public override Links ReadJson(
            JsonReader reader,
            Type objectType,
            Links existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) => throw new NotImplementedException();
    }
}
