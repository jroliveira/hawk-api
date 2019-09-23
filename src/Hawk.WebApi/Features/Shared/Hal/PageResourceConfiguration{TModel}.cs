namespace Hawk.WebApi.Features.Shared.Hal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Hal.Link;
    using Hawk.WebApi.Infrastructure.Hal.Page;
    using Hawk.WebApi.Infrastructure.Hal.Resource;
    using Hawk.WebApi.Infrastructure.Pagination;

    using Microsoft.AspNetCore.Http;

    using static DefaultLinks;

    internal sealed class PageResourceConfiguration<TModel> : IResourceConfiguration
    {
        internal PageResourceConfiguration(
            Func<HttpContext, TModel, Links> getItemsLinks,
            Func<HttpContext, PageModel<TryModel<TModel>>, Links> getLinks) => this.GetBuilder = (context, @object) =>
        {
            var model = (TryModel<PageModel<TryModel<TModel>>>)@object;

            return new Resource<TryModel<Page<Resource<TryModel<TModel>>>>>(
                model.Select(page => new Page<Resource<TryModel<TModel>>>(
                    "items",
                    page.Data.Select(item => new Resource<TryModel<TModel>>(item, item.Match(
                        _ => DocumentationLinks,
                        current => getItemsLinks(context, current)))),
                    page.Skip,
                    page.Limit)),
                model.Match(
                    _ => DocumentationLinks,
                    page => new List<Link>(PaginationLinks(context, page, getLinks(context, page)))));
        };

        public Type Type => typeof(TryModel<PageModel<TryModel<TModel>>>);

        public Func<HttpContext, object, IResource> GetBuilder { get; }
    }
}
