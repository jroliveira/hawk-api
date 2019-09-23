namespace Hawk.WebApi.Features.Account.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class AccountResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<AccountModel>((_, model) => new List<Link>
            {
                new Link("/v1/account", "me", Get),
                new Link("/v1/accounts", "create-account", Post),
            }),
        };
    }
}
