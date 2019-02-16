using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool IsNotNullOrWhiteSpace(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        public static byte[] ToBytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static byte[] ToBytes(this string s, Encoding encoding)
        {
            return encoding.GetBytes(s);
        }

        public static string ToFormat(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool? ToBool(this string s, bool? defaultValue = null)
        {
            if (bool.TryParse(s, out var value))
            {
                return new bool?(value);
            }
            return defaultValue;
        }

        public static bool? ToBoolean(this string s, bool? defaultValue = null)
        {
            return s.ToBool(defaultValue);
        }

        public static byte? ToByte(this string s, byte? defaultValue = null)
        {
            if (byte.TryParse(s, out var value))
            {
                return new byte?(value);
            }
            return defaultValue;
        }

        public static short? ToShort(this string s, short? defaultValue = null)
        {
            if (short.TryParse(s, out var value))
            {
                return new short?(value);
            }
            return defaultValue;
        }

        public static short? ToInt16(this string s, short? defaultValue = null)
        {
            return s.ToShort(defaultValue);
        }

        public static int? ToInt(this string s, int? defaultValue = null)
        {
            if (int.TryParse(s, out var value))
            {
                return new int?(value);
            }
            return defaultValue;
        }

        public static int? ToInt32(this string s, int? defaultValue = null)
        {
            return s.ToInt(defaultValue);
        }

        public static long? ToLong(this string s, long? defaultValue = null)
        {
            if (long.TryParse(s, out var value))
            {
                return new long?(value);
            }
            return defaultValue;
        }

        public static long? ToInt64(this string s, long? defaultValue = null)
        {
            return s.ToLong(defaultValue);
        }

        public static float? ToFloat(this string s, float? defaultValue = null)
        {
            if (float.TryParse(s, out var value))
            {
                return new float?(value);
            }
            return defaultValue;
        }

        public static float? ToSingle(this string s, float? defaultValue = null)
        {
            return s.ToFloat(defaultValue);
        }

        public static double? ToDouble(this string s, double? defaultValue = null)
        {
            if (double.TryParse(s, out var value))
            {
                return new double?(value);
            }
            return defaultValue;
        }

        public static decimal? ToDecimal(this string s, decimal? defaultValue = null)
        {
            if (decimal.TryParse(s, out var value))
            {
                return new decimal?(value);
            }
            return defaultValue;
        }

        public static Guid? ToGuid(this string s, Guid? defaultValue = null)
        {
            if (Guid.TryParse(s, out var value))
            {
                return new Guid?(value);
            }
            return defaultValue;
        }

        public static TEnum? ToEnum<TEnum>(this string s, TEnum? defaultValue = null) where TEnum : struct
        {
            if (!int.TryParse(s, out var value))
            {
                return defaultValue;
            }
            var value2 = (TEnum)Enum.ToObject(typeof(TEnum), value);
            return new TEnum?(value2);
        }

        public static string ToBase64String(this string s)
        {
            return s.ToBase64String(Encoding.UTF8);
        }

        public static string ToBase64String(this string s, Encoding encoding)
        {
            var bytes = encoding.GetBytes(s);
            return Convert.ToBase64String(bytes);
        }

        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }

        public static string ToTitleCase(this string s, string info)
        {
            return new CultureInfo(info, true).TextInfo.ToTitleCase(s);
        }

        public static byte[] FromBase64String(this string s)
        {
            return Convert.FromBase64String(s);
        }

        public static int GetByteCount(this string s)
        {
            return Encoding.UTF8.GetByteCount(s);
        }

        public static int GetByteCount(this string s, Encoding encoding)
        {
            return encoding.GetByteCount(s);
        }

        public static byte[] GetBytes(this string s)
        {
            return SafeEncoding.UTF8.GetBytes(s);
        }

        public static byte[] GetBytes(this string s, Encoding encoding)
        {
            return encoding.GetBytes(s);
        }

        /// <summary>
        ///     Adds a value before the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns></returns>
        public static string AddBefore(this string text, string value)
        {
            return text != null ? value + text : "";
        }

        /// <summary>
        ///     Likes normalize.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="normalize">if set to <c>true</c> [normalize string].</param>
        /// <returns></returns>
        public static bool Like(this string text, string searchValue, bool normalize = true)
        {
            return normalize
                ? (text?.ToUpperNormalize().Contains(searchValue?.ToUpperNormalize() ?? "") ?? false)
                : (text?.Contains(searchValue ?? "") ?? false);
        }

        /// <summary>
        ///     Pluralizes the specified count.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="count">The count.</param>
        /// <param name="plural">The plural.</param>
        /// <returns></returns>
        public static string Pluralize(this string text, int count, string plural = null)
        {
            return count < 2 ? text : (string.IsNullOrEmpty(plural) ? text + "s" : plural);
        }

        /// <summary>
        ///     Removes the diacritics.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            var formD = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var ch in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        ///     Remove diacritics and set to lower
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ToLowerNormalize(this string text)
        {
            return text?.ToLower().Trim().RemoveDiacritics();
        }

        /// <summary>
        ///     Remove diacritics and set to upper
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ToUpperNormalize(this string text)
        {
            return text?.ToUpper().Trim().RemoveDiacritics();
        }

        /// <summary>
        /// 获取完全限定名中只包含字母和数字的文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string TracFileName(this string s)
        {
            if (s.Contains("."))
            {
                return Regex.Replace(s.Substring(0, s.IndexOf('.')), "[^0-9A-Za-z]", "");
            }
            else
            {
                return Regex.Replace(s, "[^0-9A-Za-z]", "");
            }
        }

        /// <summary>
        /// 长度不足补零
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public static string RepairZero(this string s, int length, bool head = false)
        {
            if (s.Length < length)
            {
                if (head)
                {
                    var sb = new StringBuilder();
                    for (var i = 0; i < length - s.Length; i++)
                    {
                        sb.Append("0");
                    }
                    return sb.Append(s).ToString();
                }
                else
                {
                    var sb = new StringBuilder(s);
                    for (var i = 0; i < length - s.Length; i++)
                    {
                        sb.Append("0");
                    }
                    return sb.ToString();
                }
            }
            return s;
        }

        /// <summary>
        /// 截取或补全字符串到指定长度
        /// </summary>
        /// <param name="s">ASCII字符串</param>
        /// <param name="l">长度</param>
        /// <returns></returns>
        public static string Truncate(this string s, int l)
        {
            if (s.Length == l)
            {
                return s;
            }
            else if (s.Length > l)
            {
                return s.Substring(0, l);
            }
            else if (s.Length < l)
            {
                return s.RepairZero(l);
            }
            else
            {
                return s;
            }
        }

        public static string TruncateDesc(this string s, int l)
        {
            if (s.Length == l)
            {
                return s;
            }
            else if (s.Length > l)
            {
                return s.Substring(s.Length - l);
            }
            else if (s.Length < l)
            {
                return s.RepairZero(l, true);
            }
            else
            {
                return s;
            }
        }

        public static string GetHashID(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return "default";
            }
            s = s.ToLower();
            int hash;
            int i;
            for (hash = 0, i = 0; i < s.Length; ++i)
            {
                hash += s[i];
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }
            hash += (hash << 3);
            hash ^= (hash >> 11);
            return (Math.Abs(hash) % 10000).ToString();
        }

        public static string ConvertBase(this string s, int from, int to)
        {
            var num = Convert.ToInt32(s, from);
            var result = Convert.ToString(num, to);
            switch (to)
            {
                case 2:
                    return result.RepairZero(8, true);

                default:
                    break;
            }
            return result;
        }

        public static string[] Split(this string s)
        {
            return s.Split(new char[] { ',', '|', '*' });
        }

        public static List<string> SplitToList(this string s, char separator, bool lower = false)
        {
            var list = new List<string>();
            var ss = s.Split(separator);
            foreach (var item in ss)
            {
                if (item.IsNotNullOrWhiteSpace())
                {
                    if (lower)
                    {
                        list.Add(item.ToLower());
                    }
                    else
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string RemoveLastComma(this string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string RemoveLastChar(this string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            //半角转全角：
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }

        public static string ToRMB(this string s)
        {
            return s.ToDecimal(0m).Value.ToRMB();
        }
    }
}