namespace Hawk.WebApi.Features.Shared.Hal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Infrastructure.Hal.Link;
    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Microsoft.AspNetCore.Http;

    using static DefaultLinks;

    internal sealed class PageResourceConfiguration<TModel> : IResourceConfiguration
    {
        internal PageResourceConfiguration(
            Func<HttpContext, TModel, Links> getItemsLinks,
            Func<HttpContext, Page<TryModel<TModel>>, Links> getLinks) => this.GetBuilder = (context, @object) =>
        {
            var model = (TryModel<Page<TryModel<TModel>>>)@object;

            return new Resource<TryModel<Page<Resource<TryModel<TModel>>>>>(
                model.Select(page => new Page<Resource<TryModel<TModel>>>(
                    page.Data.Select(item => new Resource<TryModel<TModel>>(item, item.Match(
                        _ => DocumentationLinks,
                        current => getItemsLinks(context, current)))),
                    page.Skip,
                    page.Limit)),
                model.Match(
                    _ => DocumentationLinks,
                    page => new List<Link>(PaginationLinks(context, page, getLinks(context, page)))));
        };

        public Type Type => typeof(TryModel<Page<TryModel<TModel>>>);

        public Func<HttpContext, object, IResource> GetBuilder { get; }
    }
}
