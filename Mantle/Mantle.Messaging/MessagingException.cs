using System;
using System.Runtime.Serialization;

namespace Mantle.Messaging
{
    [Serializable]
    public class MessagingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MessagingException()
        {
        }

        public MessagingException(string message) : base(message)
        {
        }

        public MessagingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MessagingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}