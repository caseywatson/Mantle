using System;
using System.Diagnostics;
using System.Threading;
using Mantle.Extensions;

namespace Mantle.FaultTolerance.Extensions
{
    public static class ObjectExtensions
    {
        public static void TryUsingExponentialBackoffFaultStrategy<TTarget>(this TTarget target,
                                                                            Action<TTarget> toTry,
                                                                            int initialIntervalInSeconds = 2,
                                                                            int maxAttempts = 4)
        {
            toTry.Require(nameof(toTry));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult();

            try
            {
                stopwatch.Start();

                for (var i = 1; i <= maxAttempts; i++)
                {
                    try
                    {
                        toTry(target);

                        return;
                    }
                    catch (Exception ex)
                    {
                        trialResult.Exceptions.Add(stopwatch.Elapsed, ex);
                        Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(initialIntervalInSeconds, i)));
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

        public static TResult TryUsingExponentialBackoffFaultStrategy<TTarget, TResult>(this TTarget target,
                                                                                        Func<TTarget, TResult> toTry,
                                                                                        int initialIntervalInSeconds = 2,
                                                                                        int maxAttempts = 4)
        {
            toTry.Require(nameof(toTry));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult<TResult>();

            try
            {
                stopwatch.Start();

                for (var i = 1; i <= maxAttempts; i++)
                {
                    try
                    {
                        return toTry(target);
                    }
                    catch (Exception ex)
                    {
                        trialResult.Exceptions.Add(stopwatch.Elapsed, ex);
                        Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(initialIntervalInSeconds, i)));
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

        public static void TryUsingFixedBackoffFaultStrategy<TTarget>(this TTarget target,
                                                                      Action<TTarget> toTry,
                                                                      int intervalInSeconds = 2,
                                                                      int maxAttempts = 4)
        {
            toTry.Require(nameof(toTry));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult();

            try
            {
                stopwatch.Start();

                for (var i = 1; i <= maxAttempts; i++)
                {
                    try
                    {
                        toTry(target);

                        return;
                    }
                    catch (Exception ex)
                    {
                        trialResult.Exceptions.Add(stopwatch.Elapsed, ex);
                        Thread.Sleep(TimeSpan.FromSeconds(intervalInSeconds));
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

        public static TResult TryUsingFixedBackoffFaultStrategy<TTarget, TResult>(this TTarget target,
                                                                                  Func<TTarget, TResult> toTry,
                                                                                  int intervalInSeconds = 2,
                                                                                  int maxAttempts = 4)
        {
            toTry.Require(nameof(toTry));

            var stopwatch = new Stopwatch();
            var trialResult = new TrialResult<TResult>();

            try
            {
                stopwatch.Start();

                for (var i = 1; i <= maxAttempts; i++)
                {
                    try
                    {
                        return toTry(target);
                    }
                    catch (Exception ex)
                    {
                        trialResult.Exceptions.Add(stopwatch.Elapsed, ex);
                        Thread.Sleep(TimeSpan.FromSeconds(intervalInSeconds));
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