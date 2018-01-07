namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities.Payment;

    internal sealed class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            this.CreateMap<Models.Transaction.Payment, Pay>()
                .ConstructUsing(model => new Pay(new Price(model.Value, model.Currency), model.Date, model.Method));

            this.CreateMap<Pay, Models.Transaction.Payment>()
                .ConstructUsing(entity => new Models.Transaction.Payment(entity.Price.Value, entity.Date, entity.Method, entity.Price.Currency));
        }
    }
}