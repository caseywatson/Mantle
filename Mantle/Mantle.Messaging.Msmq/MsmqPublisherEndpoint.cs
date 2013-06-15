namespace Mantle.Messaging.Msmq
{
    public class MsmqPublisherEndpoint : MsmqEndpoint, IPublisherEndpoint
    {
        public IPublisherClient GetClient()
        {
            return new MsmqPublisherClient(this);
        }
    }
}