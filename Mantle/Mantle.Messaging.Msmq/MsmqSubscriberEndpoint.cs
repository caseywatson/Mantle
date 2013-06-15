namespace Mantle.Messaging.Msmq
{
    public class MsmqSubscriberEndpoint : MsmqEndpoint, ISubscriberEndpoint
    {
        public ISubscriberClient GetClient()
        {
            return new MsmqSubscriberClient(this);
        }
    }
}