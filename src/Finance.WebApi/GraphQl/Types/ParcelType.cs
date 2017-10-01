namespace Finance.WebApi.GraphQl.Types
{
    using Finance.WebApi.GraphQl.Sources;

    using GraphQL.Types;

    public class ParcelType : ObjectGraphType<Parcel>
    {
        public ParcelType()
        {
            this.Field(parcel => parcel.Number);
            this.Field(parcel => parcel.Total);
        }
    }
}