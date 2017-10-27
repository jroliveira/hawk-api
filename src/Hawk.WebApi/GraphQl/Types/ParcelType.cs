namespace Hawk.WebApi.GraphQl.Types
{
    using GraphQL.Types;

    using Hawk.WebApi.GraphQl.Sources;

    public class ParcelType : ObjectGraphType<Parcel>
    {
        public ParcelType()
        {
            this.Field(parcel => parcel.Number);
            this.Field(parcel => parcel.Total);
        }
    }
}