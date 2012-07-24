using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace NaplampaWcfHost.Util
{
    public static class CamelStringCapitalizer
    {
        public static string ToCamel(this string source)
        {
            if (source == null) return null;

            StringBuilder sb = new StringBuilder();

            bool capNext = true;
            for (int i = 0; i < source.Length; i++)
            {
                sb.Append(capNext ? Char.ToUpper(source[i]) : Char.ToLower(source[i]));
                capNext = (source[i] == ' ') || (source[i] == '.') || (source[i] == '-');
            }

            return sb.ToString();
        }
    }
}
