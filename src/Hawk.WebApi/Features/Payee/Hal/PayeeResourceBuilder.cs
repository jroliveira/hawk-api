namespace Hawk.WebApi.Features.Payee.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class PayeeResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<PayeeModel>((_, model) => new List<Link>
            {
                new Link($"/v1/payees/{model.Name}", "self", Get),
                new Link($"/v1/payees/{model.Name}/categories", "categories", Get),
                new Link($"/v1/payees/{model.Name}/payment-methods", "payment-methods", Get),
                new Link($"/v1/payees/{model.Name}/tags", "tags", Get),
                new Link("/v1/payees", "all", Get),
                new Link("/v1/payees", "create-payee", Post),
                new Link($"/v1/payees/{model.Name}", "update-payee", Put),
                new Link($"/v1/payees/{model.Name}", "delete-payee", Delete),
            }),

            new PageResourceConfiguration<PayeeModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/payees/{model.Name}", "self", Get),
                    new Link($"/v1/payees/{model.Name}/categories", "categories", Get),
                    new Link($"/v1/payees/{model.Name}/payment-methods", "payment-methods", Get),
                    new Link($"/v1/payees/{model.Name}/tags", "tags", Get),
                    new Link("/v1/payees", "all", Get),
                    new Link("/v1/payees", "create-payee", Post),
                    new Link($"/v1/payees/{model.Name}", "update-payee", Put),
                    new Link($"/v1/payees/{model.Name}", "delete-payee", Delete),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/payees", "self", Get),
                }),
        };
    }
}
