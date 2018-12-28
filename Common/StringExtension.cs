using System.Globalization;
using System.Security.Cryptography;
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

        #region hash

        public static byte[] ToMD5Cng(this string s)
        {
            using (var md5 = new MD5Cng())
            {
                return md5.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToMD5Cng(this string s, Encoding encoding)
        {
            using (var md5 = new MD5Cng())
            {
                return md5.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToRIPEMD160Managed(this string s)
        {
            using (var ripemd160 = new RIPEMD160Managed())
            {
                return ripemd160.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToRIPEMD160Managed(this string s, Encoding encoding)
        {
            using (var ripemd160 = new RIPEMD160Managed())
            {
                return ripemd160.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToSHA1Cng(this string s)
        {
            using (var sha1 = new SHA1Cng())
            {
                return sha1.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToSHA1Cng(this string s, Encoding encoding)
        {
            using (var sha1 = new SHA1Cng())
            {
                return sha1.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToSHA256Cng(this string s)
        {
            using (var sha256 = new SHA256Cng())
            {
                return sha256.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToSHA256Cng(this string s, Encoding encoding)
        {
            using (var sha256 = new SHA256Cng())
            {
                return sha256.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToSHA384Cng(this string s)
        {
            using (var sha384 = new SHA384Cng())
            {
                return sha384.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToSHA384Cng(this string s, Encoding encoding)
        {
            using (var sha384 = new SHA384Cng())
            {
                return sha384.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToSHA512Cng(this string s)
        {
            using (var sha512 = new SHA512Cng())
            {
                return sha512.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToSHA512Cng(this string s, Encoding encoding)
        {
            using (var sha512 = new SHA512Cng())
            {
                return sha512.ComputeHash(s.GetBytes(encoding));
            }
        }

        #endregion hash

        #region keyed hash

        public static byte[] ToHmacMD5(this string s, byte[] key)
        {
            using (var hmac = new HMACMD5(key))
            {
                return hmac.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToHmacMD5(this string s, byte[] key, Encoding encoding)
        {
            using (var hmacMD5 = new HMACMD5(key))
            {
                return hmacMD5.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToHmacRIPEMD160(this string s, byte[] key)
        {
            using (var hmac = new HMACRIPEMD160(key))
            {
                return hmac.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToHmacRIPEMD160(this string s, byte[] key, Encoding encoding)
        {
            using (var hmac = new HMACRIPEMD160(key))
            {
                return hmac.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToHmacSHA1(this string s, byte[] key)
        {
            using (var hmac = new HMACSHA1(key))
            {
                return hmac.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToHmacSHA1(this string s, byte[] key, Encoding encoding)
        {
            using (var hmac = new HMACSHA1(key))
            {
                return hmac.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToHmacSHA256(this string s, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToHmacSHA256(this string s, byte[] key, Encoding encoding)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToHmacSHA384(this string s, byte[] key)
        {
            using (var hmac = new HMACSHA384(key))
            {
                return hmac.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToHmacSHA384(this string s, byte[] key, Encoding encoding)
        {
            using (var hmac = new HMACSHA384(key))
            {
                return hmac.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToHmacSHA512(this string s, byte[] key)
        {
            using (var hmac = new HMACSHA512(key))
            {
                return hmac.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToHmacSHA512(this string s, byte[] key, Encoding encoding)
        {
            using (var hmac = new HMACSHA512(key))
            {
                return hmac.ComputeHash(s.GetBytes(encoding));
            }
        }

        public static byte[] ToMACTripleDES(this string s, byte[] key)
        {
            using (var mac = new MACTripleDES(key))
            {
                return mac.ComputeHash(s.GetBytes());
            }
        }

        public static byte[] ToMACTripleDES(this string s, byte[] key, Encoding encoding)
        {
            using (var mac = new MACTripleDES(key))
            {
                return mac.ComputeHash(s.GetBytes(encoding));
            }
        }

        #endregion keyed hash

        #region password

        public static string ToPassword(this string s)
        {
            var h = s.ToPBKDF2WithHmacSHA1(24, out var salt)
                .ToBase64String();
            return "PBKDF2WithHmacSHA1:" +
                64000 +
                ":" +
                24 +
                ":" +
                salt.ToBase64String() +
                ":" +
                32 +
                ":" +
                h;
        }

        public static string ToPassword(this string s, int saltLength)
        {
            var h = s.ToPBKDF2WithHmacSHA1(saltLength, out var salt)
                .ToBase64String();
            return "PBKDF2WithHmacSHA1:" +
                64000 +
                ":" +
                saltLength +
                ":" +
                salt.ToBase64String() +
                ":" +
                32 +
                ":" +
                h;
        }

        public static string ToPassword(this string s, byte[] salt)
        {
            return "PBKDF2WithHmacSHA1:" +
                64000 +
                ":" +
                salt.Length +
                ":" +
                salt.ToBase64String() +
                ":" +
                32 +
                ":" +
                s.ToPBKDF2WithHmacSHA1(salt).ToBase64String();
        }

        public static string ToPassword(this string s, int saltLength, int iterations, int outputBytes)
        {
            string h = s.ToPBKDF2WithHmacSHA1(saltLength, out var salt, iterations, outputBytes)
                .ToBase64String();
            return "PBKDF2WithHmacSHA1:" +
                iterations +
                ":" +
                saltLength +
                ":" +
                salt.ToBase64String() +
                ":" +
                outputBytes +
                ":" +
                h;
        }

        public static string ToPassword(this string s, byte[] salt, int iterations, int outputBytes)
        {
            return "PBKDF2WithHmacSHA1:" +
                iterations +
                ":" +
                salt.Length +
                ":" +
                salt.ToBase64String() +
                ":" +
                outputBytes +
                ":" +
                s.ToPBKDF2WithHmacSHA1(salt, iterations, outputBytes).ToBase64String();
        }

        public static bool VerifyPassword(this string s, string password)
        {
            var split = password.Split(new char[] { ':' });
            if (split.Length != 6)
            {
                throw new ArgumentOutOfRangeException("hash", password, "not allowed password hash");
            }

            if (!split[0].StartsWith("PBKDF2WithHmac"))
            {
                throw new ArgumentOutOfRangeException("hash", password, "unsupported password hash type");
            }

            var iterations = split[1].ToInt(0) ?? 0;
            if (iterations < 1)
            {
                throw new ArgumentOutOfRangeException("iterations", iterations, "iterations must >=1");
            }

            var saltLength = split[2].ToInt(0) ?? 0;
            var salt = split[3].FromBase64String();
            if (salt == null)
            {
                throw new ArgumentOutOfRangeException("salt", "no salt");
            }

            if (saltLength < 1 | salt.Length < 1)
            {
                throw new ArgumentOutOfRangeException("salt", saltLength, "salt length must >=1");
            }

            if (saltLength != salt.Length)
            {
                throw new ArgumentOutOfRangeException("salt", "the length of the salt is not equal salt length");
            }

            var outputBytes = split[4].ToInt() ?? 0;
            var hash = split[5].FromBase64String();
            if (hash == null)
            {
                throw new ArgumentOutOfRangeException("hash", "no hash");
            }

            if (outputBytes < 1 | hash.Length < 1)
            {
                throw new ArgumentOutOfRangeException("hash", outputBytes, "hash length must >= 1");
            }

            if (outputBytes != hash.Length)
            {
                throw new ArgumentOutOfRangeException("hash", "the length of the hash is not equal hash length");
            }

            var testHash = s.ToPBKDF2WithHmacSHA1(salt, iterations, outputBytes);
            return hash.HashEquals(testHash);
        }

        public static bool VerifyPassword(this string s, string password, int saltLength2, int iterations2, int outputBytes2)
        {
            var split = password.Split(new char[] { ':' });
            if (split.Length != 6)
            {
                throw new ArgumentOutOfRangeException("hash", password, "not allowed password hash");
            }

            if (!split[0].StartsWith("PBKDF2WithHmac"))
            {
                throw new ArgumentOutOfRangeException("hash", password, "unsupported password hash type");
            }

            var iterations = split[1].ToInt(0) ?? 0;
            if (iterations < 1)
            {
                throw new ArgumentOutOfRangeException("iterations", iterations, "iterations must >=1");
            }

            if (iterations != iterations2)
            {
                throw new ArgumentException("iterations is not equal the exits iterations", "iterations");
            }

            var saltLength = split[2].ToInt(0) ?? 0;
            var salt = split[3].FromBase64String();
            if (salt == null)
            {
                throw new ArgumentOutOfRangeException("salt", "no salt");
            }

            if (saltLength < 1 | salt.Length < 1)
            {
                throw new ArgumentOutOfRangeException("salt", saltLength, "salt length must >=1");
            }

            if (saltLength != salt.Length)
            {
                throw new ArgumentOutOfRangeException("salt", "the length of the salt is not equal salt length");
            }

            if (saltLength != saltLength2)
            {
                throw new ArgumentException("salt length is not equal exits salt length", "saltLength");
            }

            var outputBytes = split[4].ToInt() ?? 0;
            var hash = split[5].FromBase64String();
            if (hash == null)
            {
                throw new ArgumentOutOfRangeException("hash", "no hash");
            }

            if (outputBytes < 1 | hash.Length < 1)
            {
                throw new ArgumentOutOfRangeException("hash", outputBytes, "hash length must >= 1");
            }

            if (outputBytes != hash.Length)
            {
                throw new ArgumentOutOfRangeException("hash", "the length of the hash is not equal hash length");
            }

            if (outputBytes != outputBytes2)
            {
                throw new ArgumentException("hash length is not equal the exits hash length", "outputbytes");
            }

            var testHash = s.ToPBKDF2WithHmacSHA1(salt, iterations, outputBytes);
            return hash.HashEquals(testHash);
        }

        public static byte[] ToPBKDF2WithHmacSHA1(this string s, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(s, salt, 64000))
            {
                return pbkdf2.GetBytes(32);
            }
        }

        public static byte[] ToPBKDF2WithHmacSHA1(this string s, out byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(s, 24, 64000))
            {
                salt = pbkdf2.Salt;
                return pbkdf2.GetBytes(32);
            }
        }

        public static byte[] ToPBKDF2WithHmacSHA1(this string s, int saltLength, out byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(s, saltLength, 64000))
            {
                salt = pbkdf2.Salt;
                return pbkdf2.GetBytes(32);
            }
        }

        public static byte[] ToPBKDF2WithHmacSHA1(this string s, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(s, salt, iterations))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        public static byte[] ToPBKDF2WithHmacSHA1(this string s, out byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(s, 24, iterations))
            {
                salt = pbkdf2.Salt;
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        public static byte[] ToPBKDF2WithHmacSHA1(this string s, int saltLength, out byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(s, saltLength, iterations))
            {
                salt = pbkdf2.Salt;
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        #endregion password

        public static string ToFormat(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool? ToBool(this string s, bool? defaultValue = null)
        {
            if (bool.TryParse(s, out bool value))
            {
                return new bool?(value);
            }
            return defaultValue;
        }

        public static bool? ToBoolean(this string s, bool? defaultValue = null)
        {
            if (bool.TryParse(s, out bool value))
            {
                return new bool?(value);
            }
            return defaultValue;
        }

        public static byte? ToByte(this string s, byte? defaultValue = null)
        {
            if (byte.TryParse(s, out byte value))
            {
                return new byte?(value);
            }
            return defaultValue;
        }

        public static short? ToShort(this string s, short? defaultValue = null)
        {
            if (short.TryParse(s, out short value))
            {
                return new short?(value);
            }
            return defaultValue;
        }

        public static short? ToInt16(this string s, short? defaultValue = null)
        {
            if (short.TryParse(s, out short value))
            {
                return new short?(value);
            }
            return defaultValue;
        }

        public static int? ToInt(this string s, int? defaultValue = null)
        {
            if (int.TryParse(s, out int value))
            {
                return new int?(value);
            }
            return defaultValue;
        }

        public static int? ToInt32(this string s, int? defaultValue = null)
        {
            if (int.TryParse(s, out int value))
            {
                return new int?(value);
            }
            return defaultValue;
        }

        public static long? ToLong(this string s, long? defaultValue = null)
        {
            if (long.TryParse(s, out long value))
            {
                return new long?(value);
            }
            return defaultValue;
        }

        public static long? ToInt64(this string s, long? defaultValue = null)
        {
            if (long.TryParse(s, out long value))
            {
                return new long?(value);
            }
            return defaultValue;
        }

        public static float? ToFloat(this string s, float? defaultValue = null)
        {
            if (float.TryParse(s, out float value))
            {
                return new float?(value);
            }
            return defaultValue;
        }

        public static float? ToSingle(this string s, float? defaultValue = null)
        {
            if (float.TryParse(s, out float value))
            {
                return new float?(value);
            }
            return defaultValue;
        }

        public static double? ToDouble(this string s, double? defaultValue = null)
        {
            if (double.TryParse(s, out double value))
            {
                return new double?(value);
            }
            return defaultValue;
        }

        public static decimal? ToDecimal(this string s, decimal? defaultValue = null)
        {
            if (decimal.TryParse(s, out decimal value))
            {
                return new decimal?(value);
            }
            return defaultValue;
        }

        public static Guid? ToGuid(this string s, Guid? defaultValue = null)
        {
            if (Guid.TryParse(s, out Guid value))
            {
                return new Guid?(value);
            }
            return defaultValue;
        }

        public static TEnum? ToEnum<TEnum>(this string s, TEnum? defaultValue = null) where TEnum : struct
        {
            if (!int.TryParse(s, out int value))
            {
                return defaultValue;
            }
            var value2 = (TEnum)((object)Enum.ToObject(typeof(TEnum), value));
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
            if (count < 2) return text;
            return string.IsNullOrEmpty(plural) ? text + "s" : plural;
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
                    sb.Append(ch);
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
                for (var i = 0; i < l - s.Length; i++)
                {
                    s += "0";
                }
                return s;
            }
            else
            {
                return s;
            }
        }

        public static string TruncateDesc(this string s, int l)
        {
            var t = "";
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
                for (var i = 0; i < l - s.Length; i++)
                {
                    t += "0";
                }
                return t + s;
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
    }
}