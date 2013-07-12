using System;

namespace Mantle.Logging
{
    public class Log : ILog
    {
        public const string PublisherEndpointName = "EventLogInput";

        private readonly ILogAdapter[] logAdapters;

        public Log(ILogAdapter[] logAdapters)
        {
            if (logAdapters == null)
                throw new ArgumentNullException("logAdapters");

            if (logAdapters.Length == 0)
                throw new ArgumentException("At least one (1) log adapter must be provided.", "logAdapters");

            this.logAdapters = logAdapters;
        }

        public void Record(Event evt)
        {
            if (evt == null)
                throw new ArgumentNullException("evt");

            foreach (ILogAdapter logAdapter in logAdapters)
                logAdapter.Record(evt);
        }
    }
}