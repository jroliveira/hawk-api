namespace Hawk.Infrastructure.Logging
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    using static System.AttributeTargets;

    using static Logger;

    [AttributeUsage(Method)]
    public sealed class LogWhenInvokeAttribute : Attribute
    {
        private readonly Stopwatch stopwatch;
        private dynamic? data;

        public LogWhenInvokeAttribute() => this.stopwatch = new Stopwatch();

        public void OnEnter(
            Type declaringType,
            object instance,
            MethodBase methodBase,
            object[] values)
        {
            this.stopwatch.Start();

            this.data = new
            {
                Class = declaringType.Name,
                Method = methodBase.Name,
                Parameters = values,
            };
        }

        public void OnExit()
        {
            LogInfo("tmp", this.data);
            this.stopwatch.Stop();
        }

        public bool OnException(Exception exception) => default;
    }
}
