using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringHashExtension
    {
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
    }
}