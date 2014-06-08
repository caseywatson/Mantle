namespace Mantle.Messaging
{
    public interface ISubscriberEndpoint
    {
        string Name { get; }

        ISubscriberClient GetClient();
        ISubscriberEndpointManager GetManager();

        void Validate();
    }
}