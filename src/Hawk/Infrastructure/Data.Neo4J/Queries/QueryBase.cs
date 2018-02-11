namespace Hawk.Infrastructure.Data.Neo4J.Queries
{
    internal class QueryBase
    {
        public QueryBase(
            Database database,
            File file)
        {
            Guard.NotNull(database, nameof(database), "Database cannot be null.");
            Guard.NotNull(file, nameof(file), "File cannot be null.");

            this.Database = database;
            this.File = file;
        }

        protected Database Database { get; }

        protected File File { get; }
    }
}