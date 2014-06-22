using System.Collections.Generic;
using System.Linq;

namespace Mantle.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            source.Require("source");

            T[] sourceArray = source.ToArray();

            for (int i = 0; i < sourceArray.Length; i += chunkSize)
                yield return sourceArray.Skip(i).Take(chunkSize);
        }
    }
}