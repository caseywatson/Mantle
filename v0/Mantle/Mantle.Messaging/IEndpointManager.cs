namespace Mantle.Messaging
{
    public interface IEndpointManager
    {
        bool DoesExist();
        void Create();
    }
}