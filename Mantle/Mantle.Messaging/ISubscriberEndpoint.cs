namespace Mantle.Messaging
{
    public interface ISubscriberEndpoint
    {
        string Name { get; }

        ISubscriberClient GetClient();
        void Validate();
    }
}