namespace Hawk.WebApi.Infrastructure.Hal
{
    using System.Linq;

    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    using static System.Activator;
    using static System.AppDomain;

    using static ResourceBuilders;

    internal static class MvcCoreBuilderExtension
    {
        internal static IMvcCoreBuilder AddHal(this IMvcCoreBuilder @this, JsonSerializerSettings serializerSettings)
        {
            NewResourceBuilders(CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IResourceBuilder).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(type => CreateInstance(type) as IResourceBuilder)
                .SelectMany(item => item?.Builders)
                .ToDictionary(item => item.Key, item => item.Value));

            return @this
                .AddMvcOptions(options => options
                    .OutputFormatters
                    .Add(new HalJsonOutputFormatter(serializerSettings)));
        }
    }
}
