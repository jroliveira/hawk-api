namespace Hawk.WebApi.Features.PaymentMethod.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class PaymentMethodResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new PageResourceConfiguration<PaymentMethodModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/payment-methods", "all", Get),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/payment-methods", "self", Get),
                }),
        };
    }
}
