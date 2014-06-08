using System;

namespace Mantle.Logging
{
    public static class ILogExtensions
    {
        public static LogScope Scope(this ILog log, string scopeName, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(scopeName))
                throw new ArgumentException("Scope name is required.");

            return new LogScope(log, scopeName, parameters);
        }

        public static Event DebugFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(String.Format(message, parameters), Severity.Debug));

            log.Record(e);

            return e;
        }

        public static Event InformationFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(String.Format(message, parameters), Severity.Information));

            log.Record(e);

            return e;
        }

        public static Event WarningFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(String.Format(message, parameters), Severity.Warning));

            log.Record(e);

            return e;
        }

        public static Event ErrorFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(String.Format(message, parameters), Severity.Error));

            log.Record(e);

            return e;
        }

        public static Event FatalFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(String.Format(message, parameters), Severity.Fatal));

            log.Record(e);

            return e;
        }

        public static Event Debug(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(message, Severity.Debug, tags: tags));

            log.Record(e);

            return e;
        }

        public static Event Information(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(message, Severity.Information, tags: tags));

            log.Record(e);

            return e;
        }

        public static Event Warning(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(message, Severity.Warning, tags: tags));

            log.Record(e);

            return e;
        }

        public static Event Error(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(message, Severity.Error, tags: tags));

            log.Record(e);

            return e;
        }

        public static Event Fatal(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            var e = (new Event(message, Severity.Fatal, tags: tags));

            log.Record(e);

            return e;
        }

        public static Event Warning(this ILog log, string message, Exception ex, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            if (ex != null)
                message += String.Format("\n\nEXCEPTION DETAILS\n\n{0}", ex.ToDescriptionString());

            var e = (new Event(message, Severity.Warning, tags: tags));

            log.Record(e);

            return e;
        }

        public static Event Error(this ILog log, string message, Exception ex, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            if (ex != null)
                message += String.Format("\n\nEXCEPTION DETAILS\n\n{0}", ex.ToDescriptionString());

            var e = (new Event(message, Severity.Error, tags: tags));

            log.Record(e);

            return e;
        }

        public static Event Fatal(this ILog log, string message, Exception ex, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            if (ex != null)
                message += String.Format("\n\nEXCEPTION DETAILS\n\n{0}", ex.ToDescriptionString());

            var e = (new Event(message, Severity.Fatal, tags: tags));

            log.Record(e);

            return e;
        }
    }
}