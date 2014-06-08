using System;

namespace Mantle.Hosting
{
    public abstract class BaseWorker : IWorker
    {
        public event Action<string> ErrorOccurred;
        public event Action<string> MessageOccurred;

        public abstract void Start();
        public abstract void Stop();

        protected virtual void OnErrorOccurred(string errorMessage, params object[] parameters)
        {
            if (String.IsNullOrEmpty(errorMessage))
                throw new ArgumentException("Error message is required.", "errorMessage");

            if (ErrorOccurred != null)
                ErrorOccurred(String.Format(errorMessage, parameters));
        }

        protected virtual void OnMessageOccurred(string message, params object[] parameters)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("Message is required.", "message");

            if (MessageOccurred != null)
                MessageOccurred(String.Format(message, parameters));
        }
    }
}