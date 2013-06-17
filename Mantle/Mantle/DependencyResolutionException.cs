using System;
using System.Runtime.Serialization;

namespace Mantle
{
    [Serializable]
    public class DependencyResolutionException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DependencyResolutionException()
        {
        }

        public DependencyResolutionException(string message)
            : base(message)
        {
        }

        public DependencyResolutionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DependencyResolutionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}