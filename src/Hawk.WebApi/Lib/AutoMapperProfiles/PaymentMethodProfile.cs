namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities.Payment;
    using Hawk.WebApi.Models;

    internal sealed class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            this.CreateMap<PaymentMethod, Method>()
                .ConstructUsing(model => new Method(model.Name));

            this.CreateMap<Method, PaymentMethod>()
                .ConstructUsing(entity => new PaymentMethod(entity.Name, entity.Total));
        }
    }
}