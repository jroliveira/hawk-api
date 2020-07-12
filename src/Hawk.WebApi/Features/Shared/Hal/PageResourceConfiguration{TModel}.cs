namespace Hawk.WebApi.Features.Shared.Hal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Infrastructure.Hal.Link;
    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Microsoft.AspNetCore.Http;

    using static Hawk.WebApi.Features.Shared.Hal.DefaultLinks;

    internal sealed class PageResourceConfiguration<TModel> : IResourceConfiguration
    {
        internal PageResourceConfiguration(
            Func<HttpContext, TModel, Links> getItemsLinks,
            Func<HttpContext, Page<Try<TModel>>, Links> getLinks) => this.GetBuilder = (context, @object) =>
        {
            var model = (Try<Page<Try<TModel>>>)@object;

            return new Resource<Try<Page<Resource<Try<TModel>>>>>(
                model.Select(page => new Page<Resource<Try<TModel>>>(
                    page.Data.Select(item => new Resource<Try<TModel>>(
                        item,
                        item.Fold(DocumentationLinks)(current => getItemsLinks(
                            context,
                            current)))),
                    page.Skip,
                    page.Limit)),
                model
                    .Fold(DocumentationLinks)(page => new List<Link>(PaginationLinks(
                        context,
                        page,
                        getLinks(context, page)))));
        };

        public Type Type => typeof(Try<Page<Try<TModel>>>);

        public Func<HttpContext, object, IResource> GetBuilder { get; }
    }
}
