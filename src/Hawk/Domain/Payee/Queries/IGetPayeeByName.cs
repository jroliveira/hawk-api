namespace Hawk.Domain.Payee.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetPayeeByName : IQuery<GetByIdParam<string>, Payee>
    {
    }
}
