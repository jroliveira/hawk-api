namespace Hawk.Domain.Commands.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;

    public interface ICreateCommand
    {
        Task<Transaction> Execute(Transaction entity);
    }
}
