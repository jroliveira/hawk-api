namespace Hawk.WebApi.Infrastructure.Pagination
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PageModel<TModel> : IPageModel
    {
        public PageModel(
            IEnumerable<TModel> data,
            int skip,
            int limit)
        {
            this.Data = data.ToList();
            this.Skip = skip;
            this.Limit = limit;
        }

        public IReadOnlyCollection<TModel> Data { get; }

        public int TotalItems => this.Data.Count;

        public int Skip { get; }

        public int Limit { get; }

        public long Pages => this.Limit == 0 ? 1 : (long)Math.Ceiling((double)this.Data.Count / this.Limit);
    }
}
