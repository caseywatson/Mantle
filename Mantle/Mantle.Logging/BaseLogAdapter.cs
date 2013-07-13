using System;

namespace Mantle.Logging
{
    public abstract class BaseLogAdapter
    {
        public Func<Event, bool> Condition { get; set; }
    }
}