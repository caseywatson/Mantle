using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Strategies
{
    public class NullDeadLetterStrategy<T> : IDeadLetterStrategy<T>
        where T : class
    {
        public void HandleDeadLetterMessage(IMessageContext<T> messageContext)
        {
            // Poof!
        }
    }
}