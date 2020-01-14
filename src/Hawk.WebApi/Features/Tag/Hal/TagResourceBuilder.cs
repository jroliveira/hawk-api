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
            new ResourceConfiguration<TagModel>((_, model) => new List<Link>
            {
                new Link($"/v1/tags/{model.Name}", "self", Get),
                new Link("/v1/tags", "all", Get),
                new Link("/v1/tags", "create-tag", Post),
                new Link($"/v1/tags/{model.Name}", "update-tag", Put),
                new Link($"/v1/tags/{model.Name}", "delete-tag", Delete),
            }),

            new PageResourceConfiguration<TagModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/tags/{model.Name}", "self", Get),
                    new Link("/v1/tags", "all", Get),
                    new Link("/v1/tags", "create-tag", Post),
                    new Link($"/v1/tags/{model.Name}", "update-tag", Put),
                    new Link($"/v1/tags/{model.Name}", "delete-tag", Delete),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/tags", "self", Get),
                }),
        };
    }
}
