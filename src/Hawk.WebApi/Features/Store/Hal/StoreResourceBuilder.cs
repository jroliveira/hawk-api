﻿namespace Hawk.WebApi.Features.Store.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class StoreResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<StoreModel>((_, model) => new List<Link>
            {
                new Link($"/v1/stores/{model.Name}", "self", Get),
                new Link("/v1/stores", "all", Get),
            }),

            new PageResourceConfiguration<StoreModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/stores/{model.Name}", "self", Get),
                    new Link("/v1/stores", "all", Get),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/stores", "self", Get),
                }),
        };
    }
}