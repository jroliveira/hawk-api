namespace Hawk.Domain.Category.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IDeleteCategory : ICommand<DeleteParam<string>>
    {
    }
}
