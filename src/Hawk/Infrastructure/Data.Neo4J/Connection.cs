namespace Hawk.Infrastructure.Data.Neo4J
{
    internal class Connection
    {
        protected Connection(Database database, File file, string statement)
        {
            Guard.NotNull(database, nameof(database), "Database cannot be null.");
            Guard.NotNull(file, nameof(file), "Get script cannot be null.");
            Guard.NotNullNorEmpty(statement, nameof(statement), "Statement cannot be null or empty.");

            this.Database = database;
            this.Statement = file.ReadAllText(statement);
        }

        protected Database Database { get; }

        protected string Statement { get; }
    }
}
