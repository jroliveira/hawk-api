namespace Hawk.WebApi.Features.Category.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class CategoryResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<CategoryModel>((_, model) => new List<Link>
            {
                new Link($"/v1/categories/{model.Name}", "self", Get),
                new Link($"/v1/categories/{model.Name}/payment-methods", "payment-methods", Get),
                new Link($"/v1/categories/{model.Name}/tags", "tags", Get),
                new Link("/v1/categories", "all", Get),
                new Link("/v1/categories", "create-payee", Post),
                new Link($"/v1/categories/{model.Name}", "update-payee", Put),
                new Link($"/v1/categories/{model.Name}", "delete-payee", Delete),
            }),

            new PageResourceConfiguration<CategoryModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/categories/{model.Name}", "self", Get),
                    new Link($"/v1/categories/{model.Name}/payment-methods", "payment-methods", Get),
                    new Link($"/v1/categories/{model.Name}/tags", "tags", Get),
                    new Link("/v1/categories", "all", Get),
                    new Link("/v1/categories", "create-payee", Post),
                    new Link($"/v1/categories/{model.Name}", "update-payee", Put),
                    new Link($"/v1/categories/{model.Name}", "delete-payee", Delete),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/categories", "self", Get),
                }),
        };
    }
}
