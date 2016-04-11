using Mantle.Extensions;
using Mantle.Hosting.Interfaces;

namespace Mantle.Hosting.Hosts
{
    public abstract class BaseWorkerHost : IWorkerHost
    {
        protected BaseWorkerHost(IWorker worker)
        {
            worker.Require(nameof(worker));

            Worker = worker;

            Worker.ErrorOccurred += OnErrorOccurred;
            Worker.MessageOccurred += OnMessageOccurred;
        }

        protected IWorker Worker { get; private set; }

        public void Start() => Worker.Start();

        public void Stop() => Worker.Stop();

        protected abstract void OnErrorOccurred(string message);
        protected abstract void OnMessageOccurred(string message);
    }
}