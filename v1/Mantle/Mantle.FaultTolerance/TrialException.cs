using System;
using System.Runtime.Serialization;

namespace Mantle.FaultTolerance
{
    [Serializable]
    public class TrialException : Exception
    {
        public TrialException(TrialResult trialResult)
            : this(trialResult, $"The requested operation failed [{trialResult.TotalAttempts}] times. " +
                                "See [TrialResult] for more details.")
        {
        }

        public TrialException(TrialResult trialResult, string message) : base(message)
        {
            TrialResult = trialResult;
        }

        protected TrialException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public TrialResult TrialResult { get; set; }
    }
}