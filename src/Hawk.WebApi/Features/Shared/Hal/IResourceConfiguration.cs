namespace Hawk.WebApi.Features.Shared.Hal
{
    using System;

    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Microsoft.AspNetCore.Http;

    internal interface IResourceConfiguration
    {
        Type Type { get; }

        Func<HttpContext, object, IResource> GetBuilder { get; }
    }
}
