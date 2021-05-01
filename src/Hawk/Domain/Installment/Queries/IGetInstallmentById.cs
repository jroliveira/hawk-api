namespace Hawk.Domain.Installment.Queries
{
    using System;

    using Hawk.Domain.Shared.Queries;

    public interface IGetInstallmentById : IQuery<GetByIdParam<Guid>, Installment>
    {
    }
}
