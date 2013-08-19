namespace Mantle.Messaging.Msmq
{
    public class MsmqPublisherEndpointManager : MsmqEndpointManager, IPublisherEndpointManager
    {
        public MsmqPublisherEndpointManager(MsmqPublisherEndpoint endpoint)
            : base(endpoint)
        {
        }
    }
}