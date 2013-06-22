using System;

namespace Mantle.Hosting
{
    public abstract class BaseWorkerHost<TWorker> : IWorkerHost<TWorker>
        where TWorker : IWorker
    {
        private readonly IDependencyResolver dependencyResolver;

        protected BaseWorkerHost(IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
                throw new ArgumentNullException("dependencyResolver");

            this.dependencyResolver = dependencyResolver;

            Setup();
        }

        protected TWorker Worker { get; private set; }

        public virtual void Start()
        {
            Worker.Start();
        }

        public virtual void Stop()
        {
            Worker.Stop();
        }

        protected abstract void OnErrorOccurred(string message);
        protected abstract void OnMessageOccurred(string message);

        protected virtual T Get<T>()
        {
            return dependencyResolver.Get<T>();
        }

        protected virtual void Setup()
        {
            Worker = Get<TWorker>();

            Worker.ErrorOccurred += OnErrorOccurred;
            Worker.MessageOccurred += OnMessageOccurred;
        }
    }
}