namespace Mantle.Messaging
{
    public interface ISubscriberEndpointManager : IEndpointManager
    {
        void Create<T>();
    }
}