namespace Hawk.WebApi.Lib.Mappings
{
    using System.Linq;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using PaymentMethod = Hawk.Domain.PaymentMethod.PaymentMethod;

    internal static class PaymentMethodMapping
    {
        internal static Paged<Models.PaymentMethod.Get.PaymentMethod> ToModel(this Paged<Try<(PaymentMethod Method, uint Count)>> @this)
        {
            var model = @this
                .Data
                .Select(item => item.GetOrElse(default))
                .Select(item => new Models.PaymentMethod.Get.PaymentMethod(item.Method, item.Count))
                .ToList();

            return new Paged<Models.PaymentMethod.Get.PaymentMethod>(model, @this.Skip, @this.Limit);
        }
    }
}
