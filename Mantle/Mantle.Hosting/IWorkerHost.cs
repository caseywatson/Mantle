namespace Mantle.Hosting
{
    public interface IWorkerHost<T>
        where T : IWorker
    {
        void Start();
        void Stop();
    }
}