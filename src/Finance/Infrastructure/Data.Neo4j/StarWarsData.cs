namespace Finance.Infrastructure.Data.Neo4j
{
    using System.Threading.Tasks;

    using Finance.Sources;

    public class StarWarsData
    {
        public virtual async Task<Droid> GetByIdAsync(string id)
        {
            return await Task.Run(() => new Droid
            {
                Id = id,
                Name = "R2-D2"
            });
        }
    }
}
