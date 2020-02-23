namespace Hawk.Domain.Transaction.Commands
{
    using System;

    using Hawk.Domain.Shared.Commands;

    public interface IDeleteTransaction : ICommand<DeleteParam<Guid>>
    {
    }
}
