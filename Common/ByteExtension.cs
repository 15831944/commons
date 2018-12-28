using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class ByteExtension
    {
        public static string ToHexString(this byte[] b)
        {
            return BitConverter.ToString(b);
        }

        public static string ToString(this byte[] b)
        {
            return b.ToString(Encoding.UTF8);
        }

        public static string ToString(this byte[] b, Encoding encoding)
        {
            return encoding.GetString(b);
        }

        public static int ToInt32(this byte[] b)
        {
            //如果传入的字节数组长度小于4,则返回0
            if (b.Length < 4)
            {
                return 0;
            }

            //定义要返回的整数
            int num = 0;

            //如果传入的字节数组长度大于4,需要进行处理
            if (b.Length >= 4)
            {
                //创建一个临时缓冲区
                byte[] tempBuffer = new byte[4];

                //将传入的字节数组的前4个字节复制到临时缓冲区
                Buffer.BlockCopy(b, 0, tempBuffer, 0, 4);

                //将临时缓冲区的值转换成整数，并赋给num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //返回整数
            return num;
        }

        public static string ToBase64String(this byte[] b)
        {
            return Convert.ToBase64String(b);
        }

        public static string ToBase64String(this byte[] b, Base64FormattingOptions options)
        {
            return Convert.ToBase64String(b, options);
        }

        public static string ToBase64String(this byte[] b, int offset, int length)
        {
            return Convert.ToBase64String(b, offset, length);
        }

        public static string ToBase64String(this byte[] b, int offset, int length, Base64FormattingOptions options)
        {
            return Convert.ToBase64String(b, offset, length, options);
        }

        public static string ToBase16String(this byte[] binary, string Alphabet = "0123456789ABCDEF")
        {
            var chars = new char[binary.Length * 2];
            for (int i = 0, b; i < binary.Length; ++i)
            {
                b = binary[i];
                chars[i * 2] = Alphabet[b >> 4];
                chars[i * 2 + 1] = Alphabet[b & 0xF];
            }
            return new string(chars);
        }

        public static string ToHexUppercaseBase16String(this byte[] binary)
        {
            return binary.ToBase16String();
        }

        public static string ToHexLowercaseBase16String(this byte[] binary)
        {
            return binary.ToBase16String("0123456789abcdef");
        }

        public static string ToHexYubiModhexBase16String(this byte[] binary)
        {
            return binary.ToBase16String("cbdefghijklnrtuv");
        }

        public static string ToBase32String(this byte[] data, string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", char Special = '=')
        {
            var dataLength = data.Length;
            var result = new StringBuilder((dataLength + 4) / 5 * 8);

            byte x1, x2;
            int i;

            var length5 = (dataLength / 5) * 5;
            for (i = 0; i < length5; i += 5)
            {
                x1 = data[i];
                result.Append(Alphabet[x1 >> 3]);

                x2 = data[i + 1];
                result.Append(Alphabet[((x1 << 2) & 0x1C) | (x2 >> 6)]);
                result.Append(Alphabet[(x2 >> 1) & 0x1F]);

                x1 = data[i + 2];
                result.Append(Alphabet[((x2 << 4) & 0x10) | (x1 >> 4)]);

                x2 = data[i + 3];
                result.Append(Alphabet[((x1 << 1) & 0x1E) | (x2 >> 7)]);
                result.Append(Alphabet[(x2 >> 2) & 0x1F]);

                x1 = data[i + 4];
                result.Append(Alphabet[((x2 << 3) & 0x18) | (x1 >> 5)]);
                result.Append(Alphabet[x1 & 0x1F]);
            }

            switch (dataLength - length5)
            {
                case 1:
                    x1 = data[i];
                    result.Append(Alphabet[x1 >> 3]);
                    result.Append(Alphabet[(x1 << 2) & 0x1C]);

                    result.Append(Special, 6);
                    break;

                case 2:
                    x1 = data[i];
                    result.Append(Alphabet[x1 >> 3]);
                    x2 = data[i + 1];
                    result.Append(Alphabet[((x1 << 2) & 0x1C) | (x2 >> 6)]);
                    result.Append(Alphabet[(x2 >> 1) & 0x1F]);
                    result.Append(Alphabet[(x2 << 4) & 0x10]);

                    result.Append(Special, 4);
                    break;

                case 3:
                    x1 = data[i];
                    result.Append(Alphabet[x1 >> 3]);
                    x2 = data[i + 1];
                    result.Append(Alphabet[((x1 << 2) & 0x1C) | (x2 >> 6)]);
                    result.Append(Alphabet[(x2 >> 1) & 0x1F]);
                    x1 = data[i + 2];
                    result.Append(Alphabet[((x2 << 4) & 0x10) | (x1 >> 4)]);
                    result.Append(Alphabet[(x1 << 1) & 0x1E]);

                    result.Append(Special, 3);
                    break;

                case 4:
                    x1 = data[i];
                    result.Append(Alphabet[x1 >> 3]);
                    x2 = data[i + 1];
                    result.Append(Alphabet[((x1 << 2) & 0x1C) | (x2 >> 6)]);
                    result.Append(Alphabet[(x2 >> 1) & 0x1F]);
                    x1 = data[i + 2];
                    result.Append(Alphabet[((x2 << 4) & 0x10) | (x1 >> 4)]);
                    x2 = data[i + 3];
                    result.Append(Alphabet[((x1 << 1) & 0x1E) | (x2 >> 7)]);
                    result.Append(Alphabet[(x2 >> 2) & 0x1F]);
                    result.Append(Alphabet[(x2 << 3) & 0x18]);

                    result.Append(Special);
                    break;
            }

            return result.ToString();
        }

        public static string ToExtendedHexBase32String(this byte[] data)
        {
            const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
            const char Special = '=';
            return data.ToBase32String(Alphabet, Special);
        }

        public static string ToBase32HexString(this byte[] data)
        {
            return data.ToExtendedHexBase32String();
        }

        public static bool HashEquals(this byte[] b, byte[] a)
        {
            var diff = b.Length ^ a.Length;
            for (var i = 0; i < b.Length && i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

        #region hash

        public static byte[] ToMD5(this byte[] s)
        {
            using (var md5 = new MD5Cng())
            {
                return md5.ComputeHash(s);
            }
        }

        public static byte[] ToRIPEMD160(this byte[] s)
        {
            using (var ripemd160 = new RIPEMD160Managed())
            {
                return ripemd160.ComputeHash(s);
            }
        }

        public static byte[] ToSHA1(this byte[] s)
        {
            using (var sha1 = new SHA1Cng())
            {
                return sha1.ComputeHash(s);
            }
        }

        public static byte[] ToSHA256(this byte[] s)
        {
            using (var sha256 = new SHA256Cng())
            {
                return sha256.ComputeHash(s);
            }
        }

        public static byte[] ToSHA384(this byte[] s)
        {
            using (var sha384 = new SHA384Cng())
            {
                return sha384.ComputeHash(s);
            }
        }

        public static byte[] ToSHA512(this byte[] s)
        {
            using (var sha512 = new SHA512Cng())
            {
                return sha512.ComputeHash(s);
            }
        }

        #endregion hash

        #region keyed hash

        public static byte[] ToHmacMD5(this byte[] s, byte[] key)
        {
            using (var hmac = new HMACMD5(key))
            {
                return hmac.ComputeHash(s);
            }
        }

        public static byte[] ToHmacRIPEMD160(this byte[] s, byte[] key)
        {
            using (var hmac = new HMACRIPEMD160(key))
            {
                return hmac.ComputeHash(s);
            }
        }

        public static byte[] ToHmacSHA1(this byte[] s, byte[] key)
        {
            using (var hmac = new HMACSHA1(key))
            {
                return hmac.ComputeHash(s);
            }
        }

        public static byte[] ToHmacSHA256(this byte[] s, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(s);
            }
        }

        public static byte[] ToHmacSHA384(this byte[] s, byte[] key)
        {
            using (var hmac = new HMACSHA384(key))
            {
                return hmac.ComputeHash(s);
            }
        }

        public static byte[] ToHmacSHA512(this byte[] s, byte[] key)
        {
            using (var hmac = new HMACSHA512(key))
            {
                return hmac.ComputeHash(s);
            }
        }

        public static byte[] ToMACTripleDES(this byte[] s, byte[] key)
        {
            using (var mac = new MACTripleDES(key))
            {
                return mac.ComputeHash(s);
            }
        }

        #endregion keyed hash

        public static byte[] ToPBKDF2WithHmacSHA1(this byte[] s, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(s, salt, 64000))
            {
                return pbkdf2.GetBytes(32);
            }
        }

        public static byte[] ToPBKDF2WithHmacSHA1(this byte[] s, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(s, salt, iterations))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}