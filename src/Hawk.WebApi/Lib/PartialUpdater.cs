namespace Hawk.WebApi.Lib
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal sealed class PartialUpdater
    {
        public void Apply<T>(dynamic partialModel, T destination)
        {
            var partialModelAsJToken = JToken.FromObject(partialModel);

            using (var reader = partialModelAsJToken.CreateReader())
            {
                JsonSerializer.CreateDefault().Populate(reader, destination);
            }
        }
    }
}