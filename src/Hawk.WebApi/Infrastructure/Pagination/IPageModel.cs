namespace Hawk.WebApi.Infrastructure.Pagination
{
    public interface IPageModel
    {
        int TotalItems { get; }

        int Skip { get; }

        int Limit { get; }

        long Pages { get; }
    }
}
