namespace Hawk.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public sealed class Paged<T>
    {
        public Paged(IReadOnlyCollection<T> data, int skip, int limit)
        {
            this.Data = data;
            this.Skip = skip;
            this.Limit = limit;
        }

        public IReadOnlyCollection<T> Data { get; }

        public int Skip { get; }

        public int Limit { get; }

        public long Pages => this.Limit == 0 ? 1 : (long)Math.Ceiling((double)this.Data.Count / this.Limit);
    }
}