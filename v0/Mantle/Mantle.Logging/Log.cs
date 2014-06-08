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

        public event Action<Event, Exception> ErrorOccurred;

        public void Record(Event evt)
        {
            if (evt == null)
                throw new ArgumentNullException("evt");

            try
            {
                foreach (ILogAdapter logAdapter in logAdapters)
                {
                    if ((logAdapter.Condition == null) || (logAdapter.Condition(evt)))
                        logAdapter.Record(evt);
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred(evt, ex);
            }
        }

        private void OnErrorOccurred(Event evt, Exception ex)
        {
            if (ErrorOccurred != null)
                ErrorOccurred(evt, ex);
        }
    }
}