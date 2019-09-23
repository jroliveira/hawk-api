﻿namespace Hawk.WebApi.Features.Shared.Hal
{
    using System;

    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Hal.Link;
    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Microsoft.AspNetCore.Http;

    using static DefaultLinks;

    internal sealed class ResourceConfiguration<TModel> : IResourceConfiguration
    {
        internal ResourceConfiguration(Func<HttpContext, TModel, Links> getLinks) => this.GetBuilder = (context, @object) =>
        {
            var model = (TryModel<TModel>)@object;

            return new Resource<TryModel<TModel>>(
                model,
                model.Match(
                    _ => DocumentationLinks,
                    item => getLinks(context, item)));
        };

        public Type Type => typeof(TryModel<TModel>);

        public Func<HttpContext, object, IResource> GetBuilder { get; }
    }
}
