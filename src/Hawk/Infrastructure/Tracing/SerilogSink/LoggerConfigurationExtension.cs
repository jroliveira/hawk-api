namespace Hawk.Infrastructure.Tracing.SerilogSink
{
    using System;

    using Serilog;
    using Serilog.Configuration;

    using static OpenTracing.Util.GlobalTracer;

    public static class LoggerConfigurationExtension
    {
        public static LoggerConfiguration OpenTracing(this LoggerSinkConfiguration @this, IFormatProvider? formatProvider = default) => @this
            .Sink(new OpenTracingSink(Instance, formatProvider));
    }
}
