namespace Hawk.WebApi.Infrastructure.Hal.Resource
{
    using System;
    using System.Linq;

    using Newtonsoft.Json;

    using static Newtonsoft.Json.Linq.JObject;

    internal sealed class ResourceJsonConverter : JsonConverter<IResource<object>>
    {
        public override void WriteJson(
            JsonWriter writer,
            IResource<object> resource,
            JsonSerializer serializer)
        {
            var jsonObject = FromObject(resource.Get(), serializer);

            var links = resource.GetLinks();
            if (links != default && links.Any())
            {
                jsonObject.Add("_links", FromObject(links, serializer));
            }

            jsonObject.WriteTo(writer);
        }

        public override IResource<object> ReadJson(
            JsonReader reader,
            Type objectType,
            IResource<object> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) => throw new NotImplementedException();
    }
}
