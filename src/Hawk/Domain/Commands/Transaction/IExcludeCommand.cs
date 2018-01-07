namespace Hawk.Domain.Commands.Transaction
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;

    public interface IExcludeCommand
    {
        Task Execute(Transaction entity);
    }
}