using System;
using System.Runtime.Serialization;

namespace Mantle.Hosting
{
    [Serializable]
    public class HostingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public HostingException()
        {
        }

        public HostingException(string message) : base(message)
        {
        }

        public HostingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected HostingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}