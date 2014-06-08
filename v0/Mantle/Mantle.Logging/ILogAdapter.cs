using System;

namespace Mantle.Logging
{
    public interface ILogAdapter : ILog
    {
        Func<Event, bool> Condition { get; }
    }
}