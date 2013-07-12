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

        public static void DebugFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(String.Format(message, parameters), Severity.Debug));
        }

        public static void InformationFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(String.Format(message, parameters), Severity.Information));
        }

        public static void WarningFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(String.Format(message, parameters), Severity.Warning));
        }

        public static void ErrorFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(String.Format(message, parameters), Severity.Error));
        }

        public static void FatalFormat(this ILog log, string message, params object[] parameters)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(String.Format(message, parameters), Severity.Fatal));
        }

        public static void Debug(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(message, Severity.Debug, tags: tags));
        }

        public static void Information(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(message, Severity.Information, tags: tags));
        }

        public static void Warning(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(message, Severity.Warning, tags: tags));
        }

        public static void Error(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(message, Severity.Error, tags: tags));
        }

        public static void Fatal(this ILog log, string message, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            log.Record(new Event(message, Severity.Fatal, tags: tags));
        }

        public static void Warning(this ILog log, string message, Exception ex, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            if (ex != null)
                message += String.Format("\n\nEXCEPTION DETAILS\n\n{0}", ex.ToDescriptionString());

            log.Record(new Event(message, Severity.Warning, tags: tags));
        }

        public static void Error(this ILog log, string message, Exception ex, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            if (ex != null)
                message += String.Format("\n\nEXCEPTION DETAILS\n\n{0}", ex.ToDescriptionString());

            log.Record(new Event(message, Severity.Error, tags: tags));
        }

        public static void Fatal(this ILog log, string message, Exception ex, params string[] tags)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            if (ex != null)
                message += String.Format("\n\nEXCEPTION DETAILS\n\n{0}", ex.ToDescriptionString());

            log.Record(new Event(message, Severity.Fatal, tags: tags));
        }
    }
}