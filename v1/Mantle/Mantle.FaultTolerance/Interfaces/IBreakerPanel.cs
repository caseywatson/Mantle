using System;

namespace Mantle.FaultTolerance.Interfaces
{
    public interface IBreakerPanel
    {
        bool IsBreakerTripped(string breakerName);
        void ResetBreaker(string breakerName);
        void TripBreaker(string breakerName, TimeSpan? resetAfter = null);
    }
}