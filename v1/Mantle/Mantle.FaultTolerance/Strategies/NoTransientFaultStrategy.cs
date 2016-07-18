using System;
using Mantle.FaultTolerance.Interfaces;

namespace Mantle.FaultTolerance.Strategies
{
    public class NoTransientFaultStrategy : ITransientFaultStrategy
    {
        public void Try(Action toTryAction)
        {
            toTryAction();
        }

        public T Try<T>(Func<T> toTryFunc)
        {
            return toTryFunc();
        }
    }
}