namespace Hawk.Domain.Queries.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;

    public interface IGetByNameQuery
    {
        Task<Store> GetResult(string name, string email);
    }
}