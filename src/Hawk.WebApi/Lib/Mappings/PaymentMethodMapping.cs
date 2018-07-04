namespace Hawk.WebApi.Lib.Mappings
{
    using System.Linq;
    using Hawk.Domain.Entities.Payment;
    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.WebApi.Models.PaymentMethod.Get;

    internal static class PaymentMethodMapping
    {
        public static Paged<PaymentMethod> ToModel(this Paged<Try<(Method Method, uint Count)>> @this)
        {
            var model = @this
                .Data
                .Select(item => item.GetOrElse(default))
                .Select(item => new PaymentMethod(item.Method, item.Count))
                .ToList();

            return new Paged<PaymentMethod>(model, @this.Skip, @this.Limit);
        }
    }
}