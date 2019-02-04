readonly Func<string> GetConfiguration = () => Argument("configuration", "Release");

readonly Func<BuildData, Action<Exception>> ErrorHandler = data => exception =>
{
    throw exception;
};