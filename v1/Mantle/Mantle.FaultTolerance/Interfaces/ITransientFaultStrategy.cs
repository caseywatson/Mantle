using System;

namespace Mantle.FaultTolerance.Interfaces
{
    public interface ITransientFaultStrategy
    {
        void Try(Action toTryAction);
        T Try<T>(Func<T> toTryFunc);
    }
}