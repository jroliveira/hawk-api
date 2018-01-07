namespace Hawk.Domain.Queries.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;

    public interface IGetByIdQuery
    {
        Task<Transaction> GetResult(string id, string email);
    }
}