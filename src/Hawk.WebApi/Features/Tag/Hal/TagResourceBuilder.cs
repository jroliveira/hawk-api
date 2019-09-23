namespace Hawk.WebApi.Features.Tag.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class TagResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new PageResourceConfiguration<TagModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/tags", "all", Get),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/tags", "self", Get),
                }),
        };
    }
}
