using System;
using System.Collections.Generic;
using System.Threading;
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
        }

        public bool IsBreakerTripped(string breakerName)
        {
            breakerName.Require(nameof(breakerName));

            try
            {
                panelLock.EnterUpgradeableReadLock();

                if (breakerDictionary.ContainsKey(breakerName) == false)
                    return false;

                if (breakerDictionary[breakerName] > DateTime.Now)
                    return true;

                try
                {
                    panelLock.EnterWriteLock();
                    breakerDictionary.Remove(breakerName);

                    return false;
                }
                finally
                {
                    panelLock.ExitWriteLock();
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
                panelLock.EnterUpgradeableReadLock();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}