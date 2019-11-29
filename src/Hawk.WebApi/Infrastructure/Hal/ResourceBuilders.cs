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

        public static implicit operator ResourceBuilders(Dictionary<Type, Func<HttpContext, object, IResource>> dictionary) => new ResourceBuilders(dictionary);

        internal static object GetResource(HttpContext httpContext, object @object, Type objectType)
        {
            if (resourceBuilders == null)
            {
                return @object;
            }

            return resourceBuilders.TryGetValue(objectType, out var getResource)
                ? getResource(httpContext, @object)
                : @object;
        }
    }
}
