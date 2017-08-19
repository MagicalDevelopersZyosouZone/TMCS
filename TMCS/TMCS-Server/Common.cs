using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TMCS.Server
{
    public static class Common
    {
        public static bool HasProperty(this object obj, string propName)
        {
            return obj.GetType().GetProperties().Any(prop => prop.Name.Equals(propName));
        }

        /// <summary>
        /// For the purpose of performance, multibyte chars parsing are not supported.
        /// </summary>
        public static string EscapeFileName(this string s)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                s.Replace(c.ToString(), "%" + ((int)c).ToString("X2"));
            }
            return s;
        }
        public static string UnescapeFileName(this string s)
        {
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == '%')
                {
                    var c = (char)int.Parse(s.Substring(i + 1, 2), System.Globalization.NumberStyles.HexNumber);
                    s.Remove(i, 3);
                    s.Insert(i, c.ToString());
                }
            }
            return s;
        }
    }
}
