using System.IO;

namespace Mantle.Extensions
{
    public static class StreamExtensions
    {
        public static bool TryToRewind(this Stream stream)
        {
            stream.Require("stream");

            if (stream.CanSeek)
            {
                stream.Position = 0;
                return true;
            }

            return false;
        }
    }
}