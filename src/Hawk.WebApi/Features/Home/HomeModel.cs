namespace Hawk.WebApi.Features.Home
{
    public class HomeModel
    {
        public HomeModel(string name, string env, string version)
        {
            this.Name = name;
            this.Env = env;
            this.Version = version;
        }

        public string Name { get; }

        public string Env { get; }

        public string Version { get; }
    }
}
