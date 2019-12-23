namespace Hawk.WebApi.Infrastructure.Hal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Microsoft.AspNetCore.Http;

    internal sealed class ResourceBuilders : ReadOnlyDictionary<Type, Func<HttpContext, object, IResource>>
    {
        private static ResourceBuilders? resourceBuilders;

        private ResourceBuilders(IDictionary<Type, Func<HttpContext, object, IResource>> dictionary)
            : base(dictionary) => resourceBuilders = this;

        public static void NewResourceBuilders(IDictionary<Type, Func<HttpContext, object, IResource>> dictionary) => resourceBuilders = new ResourceBuilders(dictionary);

        internal static object GetResource(HttpContext httpContext, object @object, Type objectType)
        {
            if (resourceBuilders == default)
            {
                return @object;
            }

            return resourceBuilders.TryGetValue(objectType, out var getResource)
                ? getResource(httpContext, @object)
                : @object;
        }
    }
}
