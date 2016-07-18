using System;
using System.Diagnostics;
using System.Threading;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;

namespace Mantle.FaultTolerance.Strategies
{
    public class ExponentialBackoffTransientFaultStrategy : ITransientFaultStrategy
    {
        public ExponentialBackoffTransientFaultStrategy()
        {
            RetryIntervalInSeconds = 2;
            MaximumAttempts = 4;
        }

        [Configurable]
        public int RetryIntervalInSeconds { get; set; }

        [Configurable]
        public int MaximumAttempts { get; set; }

        public void Try(Action toTry)
        {
            toTry.Require(nameof(toTry));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult();

            try
            {
                stopwatch.Start();

                for (var i = 1; i <= MaximumAttempts; i++)
                {
                    try
                    {
                        toTry();

                        return;
                    }
                    catch (Exception ex)
                    {
                        trialResult.Exceptions.Add(stopwatch.Elapsed, ex);
                        Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(RetryIntervalInSeconds, i)));
                    }
                    finally
                    {
                        trialResult.TotalAttempts = i;
                    }
                }
            }
            finally
            {
                stopwatch.Stop();
            }

            throw new TrialException(trialResult);
        }

        public T Try<T>(Func<T> toTryFunc)
        {
            toTryFunc.Require(nameof(toTryFunc));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult<T>();

            try
            {
                stopwatch.Start();

                for (var i = 1; i <= MaximumAttempts; i++)
                {
                    try
                    {
                        return toTryFunc();
                    }
                    catch (Exception ex)
                    {
                        trialResult.Exceptions.Add(stopwatch.Elapsed, ex);
                        Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(RetryIntervalInSeconds, i)));
                    }
                    finally
                    {
                        trialResult.TotalAttempts = i;
                    }
                }
            }
            finally
            {
                stopwatch.Stop();
            }

            throw new TrialException(trialResult);
        }
    }
}