using System;
using System.Runtime.Serialization;

namespace Mantle.Storage
{
    [Serializable]
    public class StorageException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public StorageException()
        {
        }

        public StorageException(string message) : base(message)
        {
        }

        public StorageException(string message, Exception inner) : base(message, inner)
        {
        }

        protected StorageException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}