namespace Hawk.Domain.Commands.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Entities;

    public interface ICreateCommand
    {
        Task<Store> Execute(Store newEntity, Store entity);
    }
}
