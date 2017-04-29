namespace Finance.Infrastructure
{
    public class File
    {
        public virtual string ReadAllText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
    }
}