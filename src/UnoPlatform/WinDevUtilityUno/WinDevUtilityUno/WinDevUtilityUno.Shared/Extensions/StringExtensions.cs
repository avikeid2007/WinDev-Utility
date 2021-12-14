using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WinDevUtilityUno.Extensions
{
    public static class StringExtensions
    {
        public static int GetLineCount(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            using var sr = new StringReader(str);
            var count = 0;
            while (sr.ReadLine() != null)
            {
                count++;
            }
            return count;
        }
        public static async Task<int> GetLineCountAsync(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            using var sr = new StringReader(str);
            var count = 0;
            while ((await sr.ReadLineAsync()) != null)
            {
                count++;
            }
            return count;
        }

        public static IEnumerable<string> GetLines(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default;
            }
            using var sr = new StringReader(str);
            var lines = new List<string>();
            while (sr.ReadLine() is string line)
            {
                lines.Add(line);
            }
            return lines;
        }
        public static async Task<IEnumerable<string>> GetLinesAsync(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default;
            }
            using var sr = new StringReader(str);
            var lines = new List<string>();
            while ((await sr.ReadLineAsync()) is string line)
            {
                lines.Add(line);
            }
            return lines;
        }

        public static string ToFirstLower(this string str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
            {
                return char.ToLower(str[0]) + str.Substring(1);
            }
            return str;
        }
        public static string ToFirstUpper(this string str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsLower(str[0]))
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return str;
        }
        public static string RemoveExtraWhiteSpace(this string str)
        {
            while (str.Contains("  "))
            {
                str = str.Replace("  ", " ");
            }
            return str;
        }
    }
}
