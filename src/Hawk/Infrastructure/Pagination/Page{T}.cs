namespace Hawk.Infrastructure.Pagination
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Page<T> : IPage<T>
    {
        public Page(
            in IEnumerable<T> data,
            in int skip,
            in int limit)
        {
            this.Data = data.ToList();
            this.Skip = skip;
            this.Limit = limit;
        }

        public IReadOnlyCollection<T> Data { get; }

        public int Skip { get; }

        public int Limit { get; }

        public int TotalItems => this.Data.Count;

        public long Pages => this.Limit == 0 ? 1 : (long)Math.Ceiling((double)this.Data.Count / this.Limit);
    }
}
