using System;
using System.Collections.Generic;

namespace Mantle.Extensions
{
    public static class StringExtensions
    {
        public static string Merge(this string source, object data, char fieldStartDelimiter = '{',
                                   char fieldEndDelimiter = '}')
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (data == null)
                throw new ArgumentNullException("data");

            Dictionary<string, object> dataDictionary = data.ToDictionary();

            foreach (string key in dataDictionary.Keys)
                source = source.Replace((fieldStartDelimiter + key + fieldEndDelimiter), dataDictionary[key].ToString());

            return source;
        }

        public static bool? TryParseBoolean(this string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            bool temp;

            if (bool.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static DateTime? TryParseDateTime(this string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            DateTime temp;

            if (DateTime.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static double? TryParseDouble(this string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            double temp;

            if (double.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static int? TryParseInt(this string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int temp;

            if (int.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static long? TryParseLong(this string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            long temp;

            if (long.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static Guid? TryParseGuid(this string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            Guid temp;

            if (Guid.TryParse(source, out temp))
                return temp;

            return null;
        }
    }
}