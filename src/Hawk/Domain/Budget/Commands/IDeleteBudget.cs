namespace Hawk.Domain.Budget.Commands
{
    using System;

    using Hawk.Domain.Shared.Commands;

    public interface IDeleteBudget : ICommand<DeleteParam<Guid>>
    {
    }
}
