namespace Hawk.Infrastructure.Security
{
    public interface IHashAlgorithm
    {
        string Hash(string text);
    }
}
