namespace Hawk.WebApi.Infrastructure.Hal
{
    using Hawk.WebApi.Infrastructure.Hal.Link;
    using Hawk.WebApi.Infrastructure.Hal.Page;
    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Newtonsoft.Json;

    internal static class JsonSerializerSettingsExtension
    {
        internal static JsonSerializerSettings AddHal(this JsonSerializerSettings @this)
        {
            var serializerSettings = @this;
            serializerSettings.Converters.Add(new ResourceJsonConverter());
            serializerSettings.Converters.Add(new LinksJsonConverter());
            serializerSettings.Converters.Add(new PageJsonConverter());

            return serializerSettings;
        }
    }
}
