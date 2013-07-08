namespace Mantle.Messaging
{
    public abstract class Message<T>
    {
        protected Message(T payload)
        {
            Payload = payload;
        }

        public bool CanBeAbandoned { get; protected set; }
        public bool CanBeCompleted { get; protected set; }
        public bool CanBeKilled { get; protected set; }
        public bool CanGetDeliveryCount { get; protected set; }

        public T Payload { get; set; }

        public abstract int GetDeliveryCount();

        public abstract void Abandon();
        public abstract void Complete();
        public abstract void Kill();
    }
}