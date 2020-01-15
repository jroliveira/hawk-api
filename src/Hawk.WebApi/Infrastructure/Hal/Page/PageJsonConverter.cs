namespace Hawk.WebApi.Infrastructure.Hal.Page
{
    using System;

    using Hawk.Infrastructure.Pagination;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using static Newtonsoft.Json.Linq.JToken;

    internal sealed class PageJsonConverter : JsonConverter<IPage<object>>
    {
        public override void WriteJson(JsonWriter writer, IPage<object> value, JsonSerializer serializer)
        {
            var obj = new JObject
            {
                { "skip", value.Skip },
                { "limit", value.Limit },
                { "totalItems", value.TotalItems },
                { "pages", value.Pages },
                {
                    "_embedded",
                    new JObject
                    {
                        { "items", FromObject(value.Data, serializer) },
                    }
                },
            };

            obj.WriteTo(writer);
        }

        public override IPage<object> ReadJson(
            JsonReader reader,
            Type objectType,
            IPage<object> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) => throw new NotImplementedException();
    }
}
