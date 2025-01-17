using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#if SILVERLIGHT && !WindowsPhone
using System.Windows.Browser;
#endif

#if WindowsPhone
using System.Web;
#endif

#if !SILVERLIGHT && !MonoTouch  && !MonoDroid && !NETCF
using System.Web;
#endif

namespace Hammock.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullOrBlank(this string value)
        {
            return String.IsNullOrEmpty(value) ||
                   (!String.IsNullOrEmpty(value) && value.Trim() == String.Empty);
        }

        public static bool EqualsIgnoreCase(this string left, string right)
        {
            return String.Compare(left, right, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public static bool EqualsAny(this string input, params string[] args)
        {
            return args.Aggregate(false, (current, arg) => current | input.Equals(arg));
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        public static string FormatWithInvariantCulture(this string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static string Then(this string input, string value)
        {
            return String.Concat(input, value);
        }

        public static string UrlEncode(this string value)
        {
            // [DC] This is more correct than HttpUtility; it escapes spaces as %20, not +
            return Uri.EscapeDataString(value);
        }

        public static string UrlDecode(this string value)
        {
            return Uri.UnescapeDataString(value);
        }

        public static Uri AsUri(this string value)
        {
            return new Uri(value);
        }

        public static string ToBase64String(this byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static byte[] GetBytes(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static string PercentEncode(this string s)
        {
            var bytes = s.GetBytes();
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                // [DC]: Support proper encoding of special characters (\n\r\t\b)
                if((b > 7 && b < 11) || b == 13)
                {
                    sb.Append(string.Format("%0{0:X}", b));
                }
                else
                {
                    sb.Append(string.Format("%{0:X}", b));
                }
            }
            return sb.ToString();
        }

        public static IDictionary<string, string> ParseQueryString(this string query)
        {
            // [DC]: This method does not URL decode, and cannot handle decoded input
            if (query.StartsWith("?")) query = query.Substring(1);
            
            if(query.Equals(string.Empty))
            {
                return new Dictionary<string, string>();
            }

            var parts = query.Split(new[] { '&' });
            
            return parts.Select(
                part => part.Split(new[] { '=' })).ToDictionary(
                    pair => pair[0], pair => pair[1]
                );
        }

        public static bool TryParse(this string value, out double number)
        {
          try
          {
            number = double.Parse(value);
            return true;
          }
          catch
          {
            number =  0;
            return false;
          }
        }

        private const RegexOptions Options =
#if !SILVERLIGHT && !MonoTouch && !MonoDroid
            RegexOptions.Compiled | RegexOptions.IgnoreCase;
#else
            RegexOptions.IgnoreCase;
#endif
    }
}