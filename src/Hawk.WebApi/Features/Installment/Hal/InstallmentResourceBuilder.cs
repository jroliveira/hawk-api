namespace Hawk.WebApi.Features.Installment.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class InstallmentResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<InstallmentModel>((_, model) => new List<Link>
            {
                new Link($"/v1/installments/{model.Id}", "self", Get),
                new Link("/v1/installments", "all", Get),
                new Link($"/v1/installments/{model.Id}", "delete-installment", Delete),
            }),

            new PageResourceConfiguration<InstallmentModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/installments/{model.Id}", "self", Get),
                    new Link("/v1/installments", "all", Get),
                    new Link($"/v1/installments/{model.Id}", "delete-installment", Delete),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/installments", "self", Get),
                }),
        };
    }
}
