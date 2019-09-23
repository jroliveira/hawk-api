namespace Hawk.WebApi.Features.Shared.Hal
{
    using System;
    using System.Collections.Generic;

    using Hawk.WebApi.Infrastructure.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;
    using Hawk.WebApi.Infrastructure.Pagination;

    using Microsoft.AspNetCore.Http;

    using static System.Net.Http.HttpMethod;

    internal static class DefaultLinks
    {
        internal static Links DocumentationLinks => new List<Link> { new Link("/docs", "self", Get) };

        internal static Func<HttpContext, IPageModel, Links, Links> PaginationLinks => (context, page, links) =>
        {
            var next = page.Skip + page.Limit;
            var prev = page.Skip - page.Limit < 0 ? 0 : page.Skip - page.Limit;
            var last = page.TotalItems - page.Limit < 0 ? 0 : page.TotalItems - page.Limit;
            var queryString = context.GetQueryString("filter[limit]", "filter[skip]");
            var uri = context.GetUri();

            return new List<Link>(links)
            {
                new Link($"{uri}?filter[limit]={page.Limit}&filter[skip]=0{queryString}", "first", Get),
                new Link($"{uri}?filter[limit]={page.Limit}&filter[skip]={prev}{queryString}", "prev", Get),
                new Link($"{uri}?filter[limit]={page.Limit}&filter[skip]={next}{queryString}", "next", Get),
                new Link($"{uri}?filter[limit]={page.Limit}&filter[skip]={last}{queryString}", "last", Get),
            };
        };
    }
}
