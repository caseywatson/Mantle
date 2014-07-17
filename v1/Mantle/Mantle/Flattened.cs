using Mantle.Extensions;

namespace Mantle
{
    public class Flattened<T>
    {
        public Flattened()
        {
        }

        public Flattened(byte[] data)
        {
            data.Require("data");
            Data = data;
        }

        public byte[] Data { get; set; }
    }
}