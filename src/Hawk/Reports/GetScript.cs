namespace Hawk.Reports
{
    using Hawk.Infrastructure;

    internal sealed class GetScript : File
    {
        public override string ReadAllText(string path)
        {
            path = $@"Hawk.Reports.{path}";

            return base.ReadAllText(path);
        }
    }
}
