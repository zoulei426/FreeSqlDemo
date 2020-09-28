using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core
{
    public static class StringExtension
    {
        #region Methods

        public static bool IsNullOrBlank(this string source)
        {
            return source == null || string.IsNullOrEmpty(source.Trim());
        }

        public static string TrimSafe(this string source)
        {
            return source == null ? source : source.Trim();
        }

        public static string ToMD5String(this string source)
        {
            if (source == null)
                source = string.Empty;

            string str = string.Empty;
            using (MD5 md = new MD5CryptoServiceProvider())
            {
                byte[] bytes = Encoding.Default.GetBytes(source);
                byte[] buffer2 = md.ComputeHash(bytes);
                md.Clear();
                for (int i = 0; i < (buffer2.Length - 1); i++)
                    str = str + buffer2[i].ToString("x").PadLeft(2, '0');

                return str;
            }
        }

        public static string SplitIn(this string source, char inChar, int partLength)
        {
            if (partLength <= 0 || string.IsNullOrEmpty(source))
                return source;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < source.Length; i++)
            {
                sb.Append(source[i]);

                if ((i + 1) % partLength == 0 &&
                    (i + 1) != source.Length)
                    sb.Append(inChar);
            }

            return sb.ToString();
        }

        public static int? FirstInt32(this string source, int indexStart = 0)
        {
            var sb = new StringBuilder();
            for (int i = indexStart; i < source.Length; i++)
            {
                if (!source[i].IsNumber() && sb.Length > 0)
                    break;
                else if (source[i].IsNumber())
                    sb.Append(source[i]);
            }

            if (sb.Length == 0)
                return null;

            return int.Parse(sb.ToString());
        }

        public static bool IsNumber(this char source)
        {
            return source <= '9' && source >= '0';
        }

        #endregion Methods
    }
}