namespace Hawk.WebApi.Features.Shared.Hal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Microsoft.AspNetCore.Http;

    internal abstract class ResourceBuilder : IResourceBuilder
    {
        public IReadOnlyDictionary<Type, Func<HttpContext, object, IResource>> Builders => this.Configure().ToDictionary(
            configuration => configuration.Type,
            configuration => configuration.GetBuilder);

        internal abstract IReadOnlyCollection<IResourceConfiguration> Configure();
    }
}
