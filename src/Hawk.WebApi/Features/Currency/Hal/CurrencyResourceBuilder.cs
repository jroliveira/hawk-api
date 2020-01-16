namespace Hawk.WebApi.Features.Currency.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class CurrencyResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<CurrencyModel>((_, model) => new List<Link>
            {
                new Link($"/v1/currencies/{model.Name}", "self", Get),
                new Link("/v1/currencies", "all", Get),
                new Link("/v1/currencies", "create-currency", Post),
                new Link($"/v1/currencies/{model.Name}", "update-currency", Put),
                new Link($"/v1/currencies/{model.Name}", "delete-currency", Delete),
            }),

            new PageResourceConfiguration<CurrencyModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/currencies/{model.Name}", "self", Get),
                    new Link("/v1/currencies", "all", Get),
                    new Link("/v1/currencies", "create-currency", Post),
                    new Link($"/v1/currencies/{model.Name}", "update-currency", Put),
                    new Link($"/v1/currencies/{model.Name}", "delete-currency", Delete),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/currencies", "self", Get),
                }),
        };
    }
}
