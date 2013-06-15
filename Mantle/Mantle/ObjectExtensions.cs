using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mantle
{
    public static class ObjectExtensions
    {
        public static T Deserialize<T>(this Stream inputStream)
        {
            if (inputStream == null)
                throw new ArgumentNullException("inputStream");

            var serializer = new DataContractSerializer(typeof (T));

            return ((T) (serializer.ReadObject(inputStream)));
        }

        public static T DeserializeBytes<T>(this byte[] inputBytes)
        {
            if (inputBytes == null)
                throw new ArgumentNullException("inputBytes");

            return new MemoryStream(inputBytes).Deserialize<T>();
        }

        public static MemoryStream Serialize<T>(this T obj)
        {
            var outputStream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof (T));

            serializer.WriteObject(outputStream, obj);

            outputStream.Position = 0;

            return outputStream;
        }

        public static byte[] SerializeToBytes<T>(this T obj)
        {
            return obj.Serialize().ToArray();
        }
    }
}