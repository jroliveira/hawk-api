namespace Hawk.Infrastructure.Tracing.SerilogSink
{
    using System;
    using System.Collections.Generic;

    using OpenTracing;

    using Serilog.Core;
    using Serilog.Events;

    using static OpenTracing.LogFields;

    public class OpenTracingSink : ILogEventSink
    {
        private readonly ITracer tracer;
        private readonly IFormatProvider? formatProvider;

        public OpenTracingSink(ITracer tracer, IFormatProvider? formatProvider)
        {
            this.tracer = tracer;
            this.formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var span = this.tracer.ActiveSpan;
            if (span == null)
            {
                return;
            }

            var fields = new Dictionary<string, object>
            {
                { "level", logEvent.Level.ToString() },
            };

            fields[Event] = "log";

            try
            {
                fields[Message] = logEvent.RenderMessage(this.formatProvider);
                fields["message.template"] = logEvent.MessageTemplate.Text;

                if (logEvent.Exception != null)
                {
                    fields[ErrorKind] = logEvent.Exception.GetType().FullName;
                    fields[ErrorObject] = logEvent.Exception;
                }

                if (logEvent.Properties != null)
                {
                    foreach (var (key, value) in logEvent.Properties)
                    {
                        fields[key] = value;
                    }
                }
            }
            catch (Exception logException)
            {
                fields["mbv.common.logging.error"] = logException.ToString();
            }

            span.Log(fields);
        }
    }
}
