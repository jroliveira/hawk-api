namespace Hawk.Infrastructure.Data.Neo4J
{
    using System.Threading.Tasks;

    public interface ICheckNeo4J
    {
        public Task<bool> Execute();
    }
}
