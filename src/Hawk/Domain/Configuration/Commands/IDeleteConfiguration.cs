namespace Hawk.Domain.Configuration.Commands
{
    using Hawk.Domain.Shared.Commands;

    public interface IDeleteConfiguration : ICommand<DeleteParam<string>>
    {
    }
}
