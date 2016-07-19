using System;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Subscribers
{
    public abstract class BaseSubscriber<T> : ISubscriber<T>
        where T : class
    {
        public event Action<string> ErrorOccurred;
        public event Action<string> MessageOccurred;

        public abstract void HandleMessage(IMessageContext<T> messageContext);

        protected void OnErrorOccurred(IMessageContext<T> messageContext, string message, params object[] parameters)
        {
            messageContext.Require(nameof(messageContext));

            OnErrorOccurred($"[{messageContext.Id}]: {string.Format(message, parameters)}");
        }

        protected void OnErrorOccurred(string message, params object[] parameters)
        {
            message.Require(nameof(message));
            parameters.Require(nameof(parameters));

            ErrorOccurred.RaiseSafely(string.Format(message, parameters));
        }

        protected void OnMessageOccurred(IMessageContext<T> messageContext, string message, params object[] parameters)
        {
            messageContext.Require(nameof(messageContext));

            OnMessageOccurred($"[{messageContext.Id}]: {string.Format(message, parameters)}");
        }

        protected void OnMessageOccurred(string message, params object[] parameters)
        {
            message.Require(nameof(message));
            parameters.Require(nameof(parameters));

            MessageOccurred.RaiseSafely(string.Format(message, parameters));
        }
    }
}