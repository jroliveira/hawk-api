namespace Hawk.WebApi.Features.PaymentMethod
{
    using System.Linq;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;

    using static Hawk.WebApi.Features.Shared.ErrorModels.GenericErrorModel;

    public sealed class PaymentMethodModel
    {
        public PaymentMethodModel(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }

        internal static Paged<object> MapFrom(Paged<Try<(PaymentMethod Method, uint Count)>> @this)
        {
            var model = @this
                .Data
                .Select(item => item.Match(
                    HandleError,
                    paymentMethod => new PaymentMethodModel(paymentMethod.Method, paymentMethod.Count)))
                .ToList();

            return new Paged<object>(model, @this.Skip, @this.Limit);
        }
    }
}
