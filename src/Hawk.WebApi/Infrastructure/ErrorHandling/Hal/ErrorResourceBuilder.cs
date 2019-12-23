namespace Hawk.WebApi.Infrastructure.ErrorHandling.Hal
{
    using System.Collections.Generic;

    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    internal sealed class ErrorResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<Unit>((_, model) => new List<Link>()),
        };
    }
}
