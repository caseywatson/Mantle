using System;

namespace Mantle.Messaging
{
    public static class MessageExtensions
    {
        public static bool TryToAbandon<T>(this Message<T> message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (message is ICanBeAbandoned)
            {
                try
                {
                    (message as ICanBeAbandoned).Abandon();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static bool TryToComplete<T>(this Message<T> message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (message is ICanBeCompleted)
            {
                try
                {
                    (message as ICanBeCompleted).Complete();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static bool TryToRenewLock<T>(this Message<T> message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (message is ICanRenewLock)
            {
                try
                {
                    (message as ICanRenewLock).RenewLock();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static bool TryToKill<T>(this Message<T> message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (message is ICanBeKilled)
            {
                try
                {
                    (message as ICanBeKilled).Kill();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static bool TryToKillIfDeliveryCountThresholdExceeded<T>(this Message<T> message,
            int deliveryCountThreshold)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if ((message is IHaveADeliveryCount) &&
                ((message as IHaveADeliveryCount).GetDeliveryCount() > deliveryCountThreshold))
                return message.TryToKill();

            return false;
        }
    }
}