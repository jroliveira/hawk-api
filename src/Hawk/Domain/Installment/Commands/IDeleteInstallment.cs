namespace Hawk.Domain.Installment.Commands
{
    using System;

    using Hawk.Domain.Shared.Commands;

    public interface IDeleteInstallment : ICommand<DeleteParam<Guid>>
    {
    }
}
