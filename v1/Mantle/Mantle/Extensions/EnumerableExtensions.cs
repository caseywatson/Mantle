using System;
using System.Collections.Generic;
using System.Linq;

namespace Mantle.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            source.Require("source");

            var sourceArray = source.ToArray();

            for (var i = 0; i < sourceArray.Length; i += chunkSize)
                yield return sourceArray.Skip(i).Take(chunkSize);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return ((source == null) || (source.None()));
        }

        public static bool None<T>(this IEnumerable<T> source)
        {
            source.Require("source");

            return (source.Any() == false);
        }

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            source.Require("source");
            condition.Require("condition");

            return (source.Any(condition) == false);
        }
    }
}