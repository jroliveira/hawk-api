namespace Hawk.Infrastructure
{
    using System;
    using System.IO;
    using System.Reflection;

    internal class File
    {
        public virtual string ReadAllText(string name)
        {
            Guard.NotNullNorEmpty(name, nameof(name), "File name cannot be null or empty.");

            var assembly = typeof(File).GetTypeInfo().Assembly;

            try
            {
                using (var stream = assembly.GetManifestResourceStream(name))
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"Cannot load file {name}.", exception);
            }
        }
    }
}