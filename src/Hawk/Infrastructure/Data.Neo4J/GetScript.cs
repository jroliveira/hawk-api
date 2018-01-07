namespace Hawk.Infrastructure.Data.Neo4J
{
    internal sealed class GetScript : File
    {
        public override string ReadAllText(string name)
        {
            name = $@"Hawk.Infrastructure.Data.Neo4J.Scripts.{name}";

            return base.ReadAllText(name);
        }
    }
}
