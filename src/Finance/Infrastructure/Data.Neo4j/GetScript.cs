namespace Finance.Infrastructure.Data.Neo4j
{
    public class GetScript : File
    {
        public override string ReadAllText(string path)
        {
            path = $@"..\Finance\Infrastructure\Data.Neo4j\Scripts\{path}";

            return base.ReadAllText(path);
        }
    }
}
