namespace Hawk.Infrastructure.Data.Neo4J.Queries
{
    internal class QueryBase
    {
        public QueryBase(
            Database database,
            File file)
        {
            this.Database = database;
            this.File = file;
        }

        protected Database Database { get; }

        protected File File { get; }
    }
}