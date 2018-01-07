namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities;

    internal sealed class ParcelProfile : Profile
    {
        public ParcelProfile()
        {
            this.CreateMap<Models.Transaction.Parcel, Parcel>()
                .ConstructUsing(model => new Parcel(model.Total, model.Number));

            this.CreateMap<Parcel, Models.Transaction.Parcel>()
                .ConstructUsing(entity => new Models.Transaction.Parcel(entity.Number, entity.Total));
        }
    }
}