using System;
using System.Runtime.Serialization;

namespace Mantle.Messaging
{
    [Serializable]
    public class MessageDeserializationException<T> : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MessageDeserializationException(Message<T> originalMessage)
        {
            OriginalMessage = originalMessage;
        }

        public MessageDeserializationException(string message, Message<T> originalMessage) : base(message)
        {
            OriginalMessage = originalMessage;
        }

        public MessageDeserializationException(string message, Exception inner, Message<T> originalMessage)
            : base(message, inner)
        {
            OriginalMessage = originalMessage;
        }

        protected MessageDeserializationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public Message<T> OriginalMessage { get; private set; }
    }
}