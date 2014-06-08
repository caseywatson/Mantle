using System;
using System.Text;

namespace Mantle
{
    public static class StringExtensions
    {
        public static string Scrub(this string source, Func<char, bool> scrubber)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (scrubber == null)
                throw new ArgumentNullException("scrubber");

            if (source.Length == 0)
                return source;

            var builder = new StringBuilder();

            for (int i = 0; i < source.Length; i++)
            {
                if (scrubber(source[i]))
                    builder.Append(source[i]);
            }

            return builder.ToString();
        }

        public static string ScrubForLettersOrDigitsOnly(this string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Scrub(Char.IsLetterOrDigit);
        }
    }
}