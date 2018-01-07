namespace Hawk.Reports
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure;
    using Hawk.Reports.Dtos;

    using Http.Query.Filter;

    public interface IGetQueryBase
    {
        Task<Paged<Item>> GetResult(string email, Filter filter);
    }
}