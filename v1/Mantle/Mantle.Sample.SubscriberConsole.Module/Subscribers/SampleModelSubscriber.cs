using System;
using Mantle.Messaging.Interfaces;
using Mantle.Sample.SubscriberConsole.Module.Models;

namespace Mantle.Sample.SubscriberConsole.Module.Subscribers
{
    public class SampleModelSubscriber : ISubscriber<SampleModel>
    {
        public void HandleMessage(IMessageContext<SampleModel> messageContext)
        {
            Console.WriteLine(messageContext.Message.SampleString);
        }
    }
}