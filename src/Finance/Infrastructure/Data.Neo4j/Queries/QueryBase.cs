namespace Finance.Infrastructure.Data.Neo4j.Queries
{
    using Finance.Infrastructure.Data.Neo4j.Mappings;

    public class QueryBase<TReturn>
    {
        public QueryBase(
            Database database,
            IMapping<TReturn> mapping,
            File file)
        {
            this.Database = database;
            this.Mapping = mapping;
            this.File = file;
        }

        protected Database Database { get; }

        protected IMapping<TReturn> Mapping { get; }

        protected File File { get; }
    }
}