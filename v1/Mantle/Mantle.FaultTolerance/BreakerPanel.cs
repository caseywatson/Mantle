using System;
using System.Collections.Generic;
using System.Threading;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;

namespace Mantle.FaultTolerance
{
    public class BreakerPanel : IBreakerPanel
    {
        private readonly Dictionary<string, DateTime> breakerDictionary;
        private readonly ReaderWriterLockSlim panelLock;

        public BreakerPanel()
        {
            breakerDictionary = new Dictionary<string, DateTime>();
            panelLock = new ReaderWriterLockSlim();

            DefaultBreakerResetTimeout = TimeSpan.FromMinutes(5);
        }

        [Configurable]
        public TimeSpan DefaultBreakerResetTimeout { get; set; }

        public bool IsBreakerTripped(string breakerName)
        {
            breakerName.Require(nameof(breakerName));

            try
            {
                panelLock.EnterUpgradeableReadLock();

                if (breakerDictionary.ContainsKey(breakerName))
                {
                    if (breakerDictionary[breakerName] > DateTime.Now)
                        return true;

                    try
                    {
                        panelLock.EnterWriteLock();
                        breakerDictionary.Remove(breakerName);
                    }
                    finally
                    {
                        panelLock.ExitWriteLock();
                    }
                }

                return false;
            }
            finally
            {
                panelLock.ExitUpgradeableReadLock();
            }
        }

        public void ResetBreaker(string breakerName)
        {
            breakerName.Require(nameof(breakerName));

            try
            {
                panelLock.EnterUpgradeableReadLock();

                if (breakerDictionary.ContainsKey(breakerName))
                {
                    try
                    {
                        panelLock.EnterWriteLock();
                        breakerDictionary.Remove(breakerName);
                    }
                    finally
                    {
                        panelLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                panelLock.ExitUpgradeableReadLock();
            }
        }

        public void TripBreaker(string breakerName, TimeSpan? resetAfter = null)
        {
            breakerName.Require(nameof(breakerName));

            try
            {
                panelLock.EnterWriteLock();
                breakerDictionary[breakerName] = DateTime.Now.Add(resetAfter ?? DefaultBreakerResetTimeout);
            }
            finally
            {
                panelLock.ExitWriteLock();
            }
        }
    }
}