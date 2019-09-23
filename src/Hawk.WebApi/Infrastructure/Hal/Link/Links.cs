namespace Hawk.WebApi.Infrastructure.Hal.Link
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal sealed class Links : ReadOnlyCollection<Link>
    {
        private Links(IList<Link> links)
            : base(links)
        {
        }

        public static implicit operator Links(List<Link> links) => new Links(links);
    }
}
