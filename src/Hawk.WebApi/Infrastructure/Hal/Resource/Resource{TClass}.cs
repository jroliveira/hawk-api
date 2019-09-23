namespace Hawk.WebApi.Infrastructure.Hal.Resource
{
    using Hawk.WebApi.Infrastructure.Hal.Link;

    internal sealed class Resource<TClass> : Resource, IResource<TClass>
        where TClass : class
    {
        private readonly TClass @class;

        internal Resource(TClass @class, Links links)
            : base(links) => this.@class = @class;

        public TClass Get() => this.@class;
    }
}
