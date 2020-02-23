namespace Hawk.Domain.PaymentMethod.Queries
{
    using Hawk.Domain.Shared.Queries;

    public interface IGetPaymentMethodByName : IQuery<GetByIdParam<string>, PaymentMethod>
    {
    }
}
