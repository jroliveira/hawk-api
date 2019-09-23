namespace Hawk.WebApi.Features.Configuration.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class ConfigurationResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<ConfigurationModel>((_, model) => new List<Link>
            {
                new Link($"/v1/configurations/{model.Description}", "self", Get),
                new Link($"/v1/configurations/{model.Description}", "update-configuration", Put),
            }),
        };
    }
}
