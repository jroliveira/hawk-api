namespace Hawk.WebApi.Features.TransactionType.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class TransactionTypeResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<TransactionTypeModel>((_, model) => new List<Link>
            {
                new Link("/v1/transaction-types", "all", Get),
            }),

            new PageResourceConfiguration<TransactionTypeModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/transaction-types", "all", Get),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/transaction-types", "self", Get),
                }),
        };
    }
}
