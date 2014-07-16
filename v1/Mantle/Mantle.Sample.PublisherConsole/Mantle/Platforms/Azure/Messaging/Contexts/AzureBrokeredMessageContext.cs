using System;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Sample.PublisherConsole.Mantle.Platforms.Azure.Messaging.Contexts
{
    public class AzureBrokeredMessageContext<T> : IMessageContext<T>
        where T : class
    {
        public AzureBrokeredMessageContext(BrokeredMessage brokeredMessage, T message)
        {
            brokeredMessage.Require("brokeredMessage");
            message.Require("message");

            BrokeredMessage = brokeredMessage;
            Message = message;
        }

        public BrokeredMessage BrokeredMessage { get; private set; }

        public int? DeliveryCount
        {
            get { return BrokeredMessage.DeliveryCount; }
        }

        public T Message { get; private set; }

        public bool TryToAbandon()
        {
            try
            {
                BrokeredMessage.Abandon();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryToComplete()
        {
            try
            {
                BrokeredMessage.Complete();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryToDeadLetter()
        {
            try
            {
                BrokeredMessage.DeadLetter();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryToRenewLock()
        {
            try
            {
                BrokeredMessage.RenewLock();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}