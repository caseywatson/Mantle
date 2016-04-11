using System;
using Mantle.Extensions;
using Mantle.Hosting.Interfaces;
using static System.String;

namespace Mantle.Hosting.Workers
{
    public abstract class BaseWorker : IWorker
    {
        public event Action<string> ErrorOccurred;
        public event Action<string> MessageOccurred;

        public abstract void Start();
        public abstract void Stop();

        protected virtual void OnErrorOccurred(string message, params object[] parameters)
        {
            message.Require(nameof(message));
            parameters.Require(nameof(parameters));

            ErrorOccurred.RaiseSafely(Format(message, parameters));
        }

        protected virtual void OnMessageOccurred(string message, params object[] parameters)
        {
            message.Require(nameof(message));
            parameters.Require(nameof(parameters));

            MessageOccurred.RaiseSafely(Format(message, parameters));
        }
    }
}