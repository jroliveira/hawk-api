namespace System.Threading.Tasks
{
    public static class Util
    {
        public static Task<TReturn> Task<TReturn>(TReturn @return) => Tasks.Task.Run(() => @return);
    }
}
