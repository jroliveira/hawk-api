namespace Hawk.WebApi.Infrastructure.Hal.Resource
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    internal interface IResourceBuilder
    {
        IReadOnlyDictionary<Type, Func<HttpContext, object, IResource>> Builders { get; }
    }
}
