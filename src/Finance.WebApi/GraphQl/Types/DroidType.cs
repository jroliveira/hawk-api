namespace Finance.WebApi.GraphQl.Types
{
    using Finance.Sources;

    using GraphQL.Types;

    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType()
        {
            this.Field(x => x.Id)
                .Description("The Id of the Droid.");

            this.Field(x => x.Name, true)
                .Description("The name of the Droid.");
        }
    }
}
