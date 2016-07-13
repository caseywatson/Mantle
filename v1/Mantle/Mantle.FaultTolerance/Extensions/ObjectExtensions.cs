using System;
using System.Diagnostics;
using System.Threading;
using Mantle.Extensions;

namespace Mantle.FaultTolerance.Extensions
{
    public static class ObjectExtensions
    {
        public static TrialResult TryUsingExponentialBackoffFaultStrategy<TTarget>(
            this TTarget target,
            Action<TTarget> toTry,
            int initialIntervalInSeconds = 2,
            int timeoutInSeconds = 30)
        {
            toTry.Require(nameof(target));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult();
            var intervalInSeconds = initialIntervalInSeconds;

            stopwatch.Start();

            while (true)
            {
                try
                {
                    toTry(target);
                    trialResult.WasSuccessful = true;

                    return trialResult;
                }
                catch (Exception ex)
                {
                    trialResult.Exceptions.Add(stopwatch.Elapsed, ex);

                    if (stopwatch.Elapsed.TotalSeconds >= timeoutInSeconds)
                        return trialResult;

                    Thread.Sleep(intervalInSeconds * 1000);

                    intervalInSeconds *= 2;
                }
                finally
                {
                    trialResult.TotalAttempts++;
                }
            }
        }

        public static TrialResult TryUsingExponentialBackoffFaultStrategy<TTarget, TResult>(
            this TTarget target,
            Func<TTarget, TResult> toTry,
            int initialIntervalInSeconds = 2,
            int timeoutInSeconds = 30)
        {
            toTry.Require(nameof(target));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult<TResult>();
            var intervalInSeconds = initialIntervalInSeconds;

            stopwatch.Start();

            while (true)
            {
                try
                {
                    trialResult.Result = toTry(target);
                    trialResult.WasSuccessful = true;

                    return trialResult;
                }
                catch (Exception ex)
                {
                    trialResult.Exceptions.Add(stopwatch.Elapsed, ex);

                    if (stopwatch.Elapsed.TotalSeconds >= timeoutInSeconds)
                        return trialResult;

                    Thread.Sleep(intervalInSeconds * 1000);

                    intervalInSeconds *= 2;
                }
                finally
                {
                    trialResult.TotalAttempts++;
                }
            }
        }

        public static TrialResult TryUsingFixedBackoffFaultStrategy<TTarget>(
            this TTarget target,
            Action<TTarget> toTry,
            int intervalInSeconds = 2,
            int timeoutInSeconds = 30)
        {
            toTry.Require(nameof(target));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult();

            stopwatch.Start();

            while (true)
            {
                try
                {
                    toTry(target);
                    trialResult.WasSuccessful = true;

                    return trialResult;
                }
                catch (Exception ex)
                {
                    trialResult.Exceptions.Add(stopwatch.Elapsed, ex);

                    if (stopwatch.Elapsed.TotalSeconds >= timeoutInSeconds)
                        return trialResult;

                    Thread.Sleep(intervalInSeconds);
                }
                finally
                {
                    trialResult.TotalAttempts++;
                }
            }
        }

        public static TrialResult TryUsingFixedBackoffFaultStrategy<TTarget, TResult>(
            this TTarget target,
            Func<TTarget, TResult> toTry,
            int intervalInSeconds = 2,
            int timeoutInSeconds = 30)
        {
            toTry.Require(nameof(target));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult<TResult>();

            stopwatch.Start();

            while (true)
            {
                try
                {
                    trialResult.Result = toTry(target);
                    trialResult.WasSuccessful = true;

                    return trialResult;
                }
                catch (Exception ex)
                {
                    trialResult.Exceptions.Add(stopwatch.Elapsed, ex);

                    if (stopwatch.Elapsed.TotalSeconds >= timeoutInSeconds)
                        return trialResult;

                    Thread.Sleep(intervalInSeconds);
                }
                finally
                {
                    trialResult.TotalAttempts++;
                }
            }
        }
    }
}