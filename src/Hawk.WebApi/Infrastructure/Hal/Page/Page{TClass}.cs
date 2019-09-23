namespace Hawk.WebApi.Infrastructure.Hal.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class Page<TClass> : IPage<TClass>
        where TClass : class
    {
        internal Page(
            string name,
            IEnumerable<TClass> data,
            int skip,
            int limit)
        {
            this.Name = name;
            this.Data = data.ToList();
            this.Skip = skip;
            this.Limit = limit;
        }

        public string Name { get; }

        public IReadOnlyCollection<TClass> Data { get; }

        public int Skip { get; }

        public int Limit { get; }

        public long Pages => this.Limit == 0 ? 1 : (long)Math.Ceiling((double)this.Data.Count / this.Limit);
    }
}
