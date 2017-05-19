namespace Finance.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Finance.Entities.Transaction.Payment;

    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            this.CreateMap<Models.PaymentMethod.Get.PaymentMethod, Method>()
                .ConstructUsing(model => new Method(model.Name));

            this.CreateMap<Method, Models.PaymentMethod.Get.PaymentMethod>()
                .ForMember(destination => destination.Name, origin => origin.MapFrom(source => source.Name));
        }
    }
}