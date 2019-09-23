namespace Hawk.WebApi.Infrastructure.ErrorHandling.TryModel
{
    using System;

    using Newtonsoft.Json;

    using static Newtonsoft.Json.Linq.JObject;

    public sealed class TryModelJsonConverter : JsonConverter<ITryModel<object>>
    {
        public override void WriteJson(JsonWriter writer, ITryModel<object> value, JsonSerializer serializer) => value
            .Match(
                _ => FromObject(_, serializer),
                _ => FromObject(_, serializer))
            .WriteTo(writer);

        public override ITryModel<object> ReadJson(
            JsonReader reader,
            Type objectType,
            ITryModel<object> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) => throw new NotImplementedException();
    }
}
