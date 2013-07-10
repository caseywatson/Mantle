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
                (message as ICanBeAbandoned).Abandon();
                return true;
            }

            return false;
        }

        public static bool TryToComplete<T>(this Message<T> message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (message is ICanBeCompleted)
            {
                (message as ICanBeCompleted).Complete();
                return true;
            }

            return false;
        }

        public static bool TryToKill<T>(this Message<T> message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (message is ICanBeKilled)
            {
                (message as ICanBeKilled).Kill();
                return true;
            }

            return false;
        }

        public static bool TryToKillIfDeliveryCountThresholdExceeded<T>(this Message<T> message, int deliveryCountThreshold)
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