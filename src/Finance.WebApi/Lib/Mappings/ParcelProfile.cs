namespace Finance.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Finance.Entities.Transaction.Installment;

    public class ParcelProfile : Profile
    {
        public ParcelProfile()
        {
            this.CreateMap<Models.Transaction.Parcel, Parcel>()
                .ConstructUsing(model => new Parcel(model.Total, model.Number));

            this.CreateMap<Parcel, Models.Transaction.Parcel>();

            this.CreateMap<Parcel, GraphQl.Sources.Parcel>();
        }
    }
}