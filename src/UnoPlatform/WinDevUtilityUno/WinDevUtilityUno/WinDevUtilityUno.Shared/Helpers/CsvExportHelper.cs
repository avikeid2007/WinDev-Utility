using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WinDevUtilityUno.Helpers
{
    public static class CsvExportHelper
    {
        public static string ExportAsText<T>(IList<T> objects, bool includeHeaderLine = true)
        {
            return Export(objects, includeHeaderLine);
        }

        public static string Export<T>(IList<T> objects, bool includeHeaderLine = true)
        {

            var sb = new StringBuilder();

            //Get properties using reflection.  
            var propertyInfos = typeof(T).GetTypeInfo();

            if (includeHeaderLine)
            {
                //add header line.  
                foreach (var propertyInfo in propertyInfos.DeclaredProperties)
                {
                    sb.Append(propertyInfo.Name).Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            //add value for each property.  
            foreach (T obj in objects)
            {
                foreach (var propertyInfo in propertyInfos.DeclaredProperties)
                {
                    sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            return sb.ToString();
        }
        public static byte[] ExportToBytes<T>(IList<T> objects, bool includeHeaderLine = true)
        {
            return Encoding.UTF8.GetBytes(Export(objects, includeHeaderLine));
        }
        private static string MakeValueCsvFriendly(object value)
        {
            if (value == null) return "";

            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd");
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string output = value.ToString();

            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output;

        }
    }
}
