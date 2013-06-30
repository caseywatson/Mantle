using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

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

        public static T DeserializeString<T>(this string source)
        {
            if (String.IsNullOrEmpty(source))
                throw new ArgumentException("Source is required.");

            var inputStream = new MemoryStream();

            using (var streamWriter = new StreamWriter(inputStream, Encoding.UTF8))
            {
                streamWriter.Write(source);
                streamWriter.Flush();

                inputStream.Position = 0;

                return inputStream.Deserialize<T>();
            }
        }

        public static MemoryStream Serialize<T>(this T obj)
        {
            var outputStream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof (T));

            serializer.WriteObject(outputStream, obj);

            outputStream.Position = 0;

            return outputStream;
        }

        public static string SerializeToString<T>(this T obj)
        {
            MemoryStream inputStream = Serialize(obj);

            inputStream.Position = 0;

            using (var streamReader = new StreamReader(inputStream, Encoding.UTF8))
                return streamReader.ReadToEnd();
        }

        public static byte[] SerializeToBytes<T>(this T obj)
        {
            return obj.Serialize().ToArray();
        }
    }
}