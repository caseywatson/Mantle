﻿using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Mantle.Extensions
{
    public static class GenericExtensions
    {
        public static Flattened<T> Flatten<T>(this T source)
            where T : class
        {
            source.Require("source");

            using (var sourceStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));

                serializer.WriteObject(sourceStream, source);
                sourceStream.TryToRewind();

                return new Flattened<T>(sourceStream.ToArray());
            }
        }

        public static T FromJson<T>(this string objectString)
        {
            objectString.Require("objectString");
            return JsonConvert.DeserializeObject<T>(objectString);
        }

        public static T FromXml<T>(this string source)
        {
            source.Require("source");

            using (var sourceStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(sourceStream))
            {
                streamWriter.Write(source);
                streamWriter.Flush();
                sourceStream.TryToRewind();

                var serializer = new DataContractSerializer(typeof(T));

                return ((T) (serializer.ReadObject(sourceStream)));
            }
        }

        public static T Inflate<T>(this Flattened<T> flattened)
            where T : class
        {
            flattened.Require("flattened");

            var serializer = new DataContractSerializer(typeof(T));
            var sourceStream = new MemoryStream(flattened.Data);

            sourceStream.TryToRewind();

            return (serializer.ReadObject(sourceStream) as T);
        }

        public static void Require<T>(this T parameter, string parameterName, string errorMessage = null)
            where T : class
        {
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException("[parameterName] is required.", "parameterName");

            if (parameter == null)
            {
                if (errorMessage == null)
                    throw new ArgumentNullException(parameterName);

                throw new ArgumentNullException(parameterName, errorMessage);
            }
        }

        public static string ToJson<T>(this T @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static string ToXml<T>(this T source)
        {
            using (var sourceStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));

                serializer.WriteObject(sourceStream, source);
                sourceStream.TryToRewind();

                using (var streamReader = new StreamReader(sourceStream))
                    return streamReader.ReadToEnd();
            }
        }
    }
}