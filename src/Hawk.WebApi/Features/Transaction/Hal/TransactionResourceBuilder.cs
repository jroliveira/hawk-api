namespace Hawk.WebApi.Features.Transaction.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class TransactionResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<TransactionModel>((_, model) => new List<Link>
            {
                new Link($"/v1/transactions/{model.Id}", "self", Get),
                new Link("/v1/transactions", "all", Get),
                new Link("/v1/transactions", "create-transaction", Post),
                new Link($"/v1/transactions/{model.Id}", "update-transaction", Put),
                new Link($"/v1/transactions/{model.Id}", "delete-transaction", Delete),
            }),

            new PageResourceConfiguration<TransactionModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/transactions/{model.Id}", "self", Get),
                    new Link("/v1/transactions", "all", Get),
                    new Link("/v1/transactions", "create-transaction", Post),
                    new Link($"/v1/transactions/{model.Id}", "update-transaction", Put),
                    new Link($"/v1/transactions/{model.Id}", "delete-transaction", Delete),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/transactions", "self", Get),
                }),
        };
    }
}
