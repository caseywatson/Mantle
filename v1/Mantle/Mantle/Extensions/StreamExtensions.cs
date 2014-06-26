using System.IO;

namespace Mantle.Extensions
{
    public static class StreamExtensions
    {
        public static void Rewind(this Stream stream)
        {
            stream.Require("stream");

            if (stream.CanSeek)
                stream.Position = 0;
        }
    }
}