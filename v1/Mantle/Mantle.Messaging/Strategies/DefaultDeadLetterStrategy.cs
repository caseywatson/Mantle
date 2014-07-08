using Mantle.Extensions;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Strategies
{
    public class DefaultDeadLetterStrategy<T> : IDeadLetterStrategy<T>
    {
        public void HandleDeadLetterMessage(IMessageContext<T> messageContext)
        {
            messageContext.Require("messageContext");
            messageContext.TryToDeadLetter();
        }
    }
}