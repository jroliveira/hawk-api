namespace Hawk.Infrastructure.Data.Neo4j
{
    public class GetScript : File
    {
        public override string ReadAllText(string name)
        {
            name = $@"Hawk.Infrastructure.Data.Neo4j.Scripts.{name}";

            return base.ReadAllText(name);
        }
    }
}
