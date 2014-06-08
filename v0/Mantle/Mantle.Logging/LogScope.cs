using System;

namespace Mantle.Logging
{
    public class LogScope : IDisposable, ILog
    {
        private readonly ILog log;
        private readonly string name;

        public LogScope(ILog log, string blockName, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(blockName))
                throw new ArgumentException("Block name is required.", "blockName");

            this.log = log;

            name = String.Format("{0} ({1})", blockName, String.Join(", ", parameters));

            log.Debug(String.Format("ENTERING [{0}]", name));
        }

        public void Dispose()
        {
            if (log != null)
                log.Debug(String.Format("LEAVING [{0}]", name));
        }

        public void Record(Event evt)
        {
            if (evt == null)
                throw new ArgumentNullException("evt");

            evt.Body += String.Format("INSIDE [{0}]\n\n", name);

            log.Record(evt);
        }
    }
}