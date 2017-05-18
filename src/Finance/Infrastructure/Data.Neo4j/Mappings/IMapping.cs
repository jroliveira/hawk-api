namespace Finance.Infrastructure.Data.Neo4j.Mappings
{
    using global::Neo4j.Driver.V1;

    public interface IMapping<out TReturn>
    {
        TReturn MapFrom(IRecord record);
    }
}
