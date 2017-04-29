namespace Finance.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Finance.Entities.Transaction.Payment;

    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            this.CreateMap<Models.Transaction.Payment, Payment>()
                .ConstructUsing(model => new Payment(model.Value, model.Date));

            this.CreateMap<Payment, Models.Transaction.Payment>()
                .ForMember(destination => destination.Method, origin => origin.MapFrom(source => source.Method.Name));
        }
    }
}