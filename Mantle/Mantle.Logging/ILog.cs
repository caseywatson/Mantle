namespace Mantle.Logging
{
    public interface ILog
    {
        void Record(Event evt);
    }
}