using Mantle.Extensions;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Strategies
{
    public class DefaultDeadLetterStrategy<T> : IDeadLetterStrategy<T>
        where T : class
    {
        public void HandleDeadLetterMessage(IMessageContext<T> messageContext)
        {
            messageContext.Require(nameof(messageContext));
            messageContext.TryToDeadLetter();
        }
    }
}