namespace Hawk.WebApi.Infrastructure.ErrorHandling.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    internal sealed class ErrorResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<Unit>((_, model) => new List<Link>()),
        };
    }
}
