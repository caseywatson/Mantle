using System;

namespace Mantle.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static string Merge(this string source, object data, char fieldStartDelimiter = '{',
            char fieldEndDelimiter = '}')
        {
            source.Require(nameof(source));
            data.Require(nameof(data));

            var dataDictionary = data.ToDictionary();

            foreach (var key in dataDictionary.Keys)
            {
                if (dataDictionary[key] != null)
                {
                    source = source.Replace((fieldStartDelimiter + key + fieldEndDelimiter),
                        dataDictionary[key].ToString());
                }
            }

            return source;
        }

        public static void Require(this string parameter, string parameterName, string errorMessage = null)
        {
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentException($"[{nameof(parameterName)}] is required.", nameof(parameterName));

            if (string.IsNullOrEmpty(parameter))
            {
                if (errorMessage == null)
                    throw new ArgumentException($"[{parameterName}] is required.", parameterName);

                throw new ArgumentException(errorMessage, parameterName);
            }
        }

        public static bool? TryParseBoolean(this string source)
        {
            source.Require(nameof(source));

            bool temp;

            if (bool.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static byte? TryParseByte(this string source)
        {
            source.Require(nameof(source));

            byte temp;

            if (byte.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static DateTime? TryParseDateTime(this string source)
        {
            source.Require(nameof(source));

            DateTime temp;

            if (DateTime.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static decimal? TryParseDecimal(this string source)
        {
            source.Require(nameof(source));

            decimal temp;

            if (decimal.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static TimeSpan? TryParseTimeSpan(this string source)
        {
            source.Require(nameof(source));

            TimeSpan temp;

            if (TimeSpan.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static double? TryParseDouble(this string source)
        {
            source.Require(nameof(source));

            double temp;

            if (double.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static float? TryParseFloat(this string source)
        {
            source.Require(nameof(source));

            float temp;

            if (float.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static Guid? TryParseGuid(this string source)
        {
            source.Require(nameof(source));

            Guid temp;

            if (Guid.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static int? TryParseInt(this string source)
        {
            source.Require(nameof(source));

            int temp;

            if (int.TryParse(source, out temp))
                return temp;

            return null;
        }

        public static long? TryParseLong(this string source)
        {
            source.Require(nameof(source));

            long temp;

            if (long.TryParse(source, out temp))
                return temp;

            return null;
        }
    }
}