namespace Hawk.Domain.Commands.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;

    public interface IExcludeCommand
    {
        Task Execute(Store entity);
    }
}