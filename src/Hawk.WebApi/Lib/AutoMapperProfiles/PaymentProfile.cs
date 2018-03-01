namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities.Payment;

    internal sealed class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            this.CreateMap<Pay, Models.Transaction.Payment>()
                .ConstructUsing(entity => new Models.Transaction.Payment(entity.Price.Value, entity.Date, entity.Method, entity.Price.Currency));
        }
    }
}