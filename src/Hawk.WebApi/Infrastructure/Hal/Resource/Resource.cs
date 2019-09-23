namespace Hawk.WebApi.Infrastructure.Hal.Resource
{
    using Hawk.WebApi.Infrastructure.Hal.Link;

    internal abstract class Resource : IResource
    {
        private readonly Links links;

        protected Resource(Links links) => this.links = links;

        public Links GetLinks() => this.links;
    }
}
