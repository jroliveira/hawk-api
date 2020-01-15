namespace Hawk.Infrastructure.Pagination
{
    public interface IPage
    {
        int Skip { get; }

        int Limit { get; }

        int TotalItems { get; }

        long Pages { get; }
    }
}
