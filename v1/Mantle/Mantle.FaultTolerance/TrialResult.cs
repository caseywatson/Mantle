using System;
using System.Collections.Generic;

namespace Mantle.FaultTolerance
{
    public class TrialResult
    {
        public TrialResult()
        {
            Exceptions = new Dictionary<TimeSpan, Exception>();
        }

        public Dictionary<TimeSpan, Exception> Exceptions { get; set; }
        public bool WasSuccessful { get; set; }
        public int TotalAttempts { get; set; }
    }

    public class TrialResult<T> : TrialResult
    {
        public T Result { get; set; }
    }
}