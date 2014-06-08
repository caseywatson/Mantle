using System;
using System.Text;

namespace Mantle.Logging
{
    public static class ExceptionExtensions
    {
        public static string ToDescriptionString(this Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            var descriptionBuilder = new StringBuilder();

            descriptionBuilder.AppendLine(String.Format("EXCEPTION MESSAGE: [{0}]", ex.Message));
            descriptionBuilder.AppendLine();

            if (ex.Data.Count > 0)
            {
                descriptionBuilder.AppendLine("EXCEPTION DATA");
                descriptionBuilder.AppendLine();

                foreach (object key in ex.Data.Keys)
                {
                    descriptionBuilder.AppendLine(String.Format("{0}: [{1}]", key, ex.Data[key]));
                    descriptionBuilder.AppendLine();
                }
            }

            if (String.IsNullOrEmpty(ex.Source) == false)
            {
                descriptionBuilder.AppendLine(String.Format("EXCEPTION SOURCE: [{0}]", ex.Source));
                descriptionBuilder.AppendLine();
            }

            if (String.IsNullOrEmpty(ex.StackTrace) == false)
            {
                descriptionBuilder.AppendLine(String.Format("EXCEPTION STACK TRACE: [{0}]", ex.StackTrace));
                descriptionBuilder.AppendLine();
            }

            if (ex.InnerException != null)
            {
                descriptionBuilder.AppendLine("INNER EXCEPTION");
                descriptionBuilder.AppendLine();
                descriptionBuilder.AppendLine(ex.InnerException.ToDescriptionString());
                descriptionBuilder.AppendLine();
            }

            return descriptionBuilder.ToString();
        }
    }
}