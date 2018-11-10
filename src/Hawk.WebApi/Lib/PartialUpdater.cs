namespace Hawk.WebApi.Lib
{
    using static Newtonsoft.Json.JsonSerializer;
    using static Newtonsoft.Json.Linq.JToken;

    internal static class PartialUpdater
    {
        internal static void Apply<T>(dynamic partialModel, T destination)
        {
            var partialModelAsJToken = FromObject(partialModel);

            using (var reader = partialModelAsJToken.CreateReader())
            {
                CreateDefault().Populate(reader, destination);
            }
        }
    }
}
