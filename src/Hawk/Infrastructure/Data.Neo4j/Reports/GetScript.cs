namespace Hawk.Infrastructure.Data.Neo4j.Reports
{
    public class GetScript : File
    {
        public override string ReadAllText(string path)
        {
            path = $@"Hawk.Infrastructure.Data.Neo4j.Reports.{path}";

            return base.ReadAllText(path);
        }
    }
}
