namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities.Payment;

    internal sealed class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            this.CreateMap<(Method Method, int Count), Models.PaymentMethod.Get.PaymentMethod>()
                .ConstructUsing(entity => new Models.PaymentMethod.Get.PaymentMethod(entity.Method.Name, entity.Count));
        }
    }
}