namespace Mantle.Messaging
{
    public interface IPublisherEndpoint
    {
        string Name { get; }

        IPublisherClient GetClient();
        void Validate();
    }
}