namespace Hawk.WebApi.Features.Home.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class HomeResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<HomeModel>((_, model) => new List<Link>
            {
                new Link("/v1", "self", Get),
                new Link("/v1/account", "me", Get),
                new Link("/v1/currencies", "currencies", Get),
                new Link("/v1/payment-methods", "payment-methods", Get),
                new Link("/v1/stores", "stores", Get),
                new Link("/v1/tags", "tags", Get),
                new Link("/v1/transactions", "transactions", Get),
            }),
        };
    }
}
