namespace Finance.Infrastructure.Data.Neo4j.Reports
{
    public class GetScript : File
    {
        public override string ReadAllText(string path)
        {
            path = $@"Finance.Infrastructure.Data.Neo4j.Reports.{path}";

            return base.ReadAllText(path);
        }
    }
}
