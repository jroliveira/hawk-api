namespace Hawk.Domain.Tag.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IDeleteTag : ICommand<DeleteParam<string>>
    {
    }
}
