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
            new ResourceConfiguration<PaymentMethodModel>((_, model) => new List<Link>
            {
                new Link($"/v1/payment-methods/{model.Name}", "self", Get),
                new Link("/v1/payment-methods", "all", Get),
                new Link("/v1/payment-methods", "create-payment-method", Post),
                new Link($"/v1/payment-methods/{model.Name}", "update-payment-method", Put),
                new Link($"/v1/payment-methods/{model.Name}", "delete-payment-method", Delete),
            }),

            new PageResourceConfiguration<PaymentMethodModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/payment-methods/{model.Name}", "self", Get),
                    new Link("/v1/payment-methods", "all", Get),
                    new Link("/v1/payment-methods", "create-payment-method", Post),
                    new Link($"/v1/payment-methods/{model.Name}", "update-payment-method", Put),
                    new Link($"/v1/payment-methods/{model.Name}", "delete-payment-method", Delete),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/payment-methods", "self", Get),
                }),
        };
    }
}
