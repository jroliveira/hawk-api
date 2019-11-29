namespace Hawk.WebApi.Features.PaymentMethod
{
    using System.Linq;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Infrastructure.Pagination;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;

    public sealed class PaymentMethodModel
    {
        public PaymentMethodModel(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }

        internal static TryModel<PageModel<TryModel<PaymentMethodModel>>> MapPaymentMethod(Page<Try<(PaymentMethod Method, uint Count)>> @this) => new PageModel<TryModel<PaymentMethodModel>>(
            @this
                .Data
                .Select(item => item.Match(
                    HandleException<PaymentMethodModel>,
                    paymentMethod => new PaymentMethodModel(paymentMethod.Method, paymentMethod.Count))),
            @this.Skip,
            @this.Limit);
    }
}
