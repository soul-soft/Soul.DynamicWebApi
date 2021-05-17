using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soul.DynamicWebApi
{
    internal static class StringExtensions
    {
        public static string LeftTrim(this string str, string trim)
        {
            var index = str.IndexOf(trim, StringComparison.CurrentCultureIgnoreCase);
            if (index >= 0)
            {
                return str.Substring(trim.Length);
            }
            return str;
        }
        public static string RightTrim(this string str, string trim)
        {
            var index = str.LastIndexOf(trim, StringComparison.CurrentCultureIgnoreCase);
            if (index >= 0)
            {
                return str.Substring(0, str.Length - trim.Length);
            }
            return str;
        }
    }
}
